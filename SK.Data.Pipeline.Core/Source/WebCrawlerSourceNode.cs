using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using SK.RetryLib;
using System.Net;
using System.IO;
using SK.Data.Pipeline.Core.Common;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace SK.Data.Pipeline.Core
{
    public class WebCrawlerSourceNode : SourceNode
    {
        private ConcurrentQueue<string> _NextUrlQueue;

        private int _MaxParallelCount = 10;

        private ConcurrentBag<string> VisitedUrls = new ConcurrentBag<string>();

        public int MaxParallelCount { 
            set
            {
                _MaxParallelCount = value;
            }

            get
            {
                return _MaxParallelCount;
            }
        }

        public XMLEntityModel EntityModel { get; set; }

        public Func<string, string, IEnumerable<string>> DiscoverCrawlerUrls { get; set; }

        private CookieContainer _CookieContainer = null;
        public CookieContainer CookieContainer
        {
            get
            {
                if (_CookieContainer == null)
                {
                    _CookieContainer = new CookieContainer();
                }

                return _CookieContainer;
            }
        }

        public WebCrawlerSourceNode(string[] rootUrls, XMLEntityModel entityModel, Func<string, string, IEnumerable<string>> discoverCrawlerUrls, CookieContainer cookieContainer = null)
        {
            _NextUrlQueue = new ConcurrentQueue<string>();
            foreach (string rootUrl in rootUrls)
            {
                _NextUrlQueue.Enqueue(rootUrl);
                VisitedUrls.Add(rootUrl);
            }

            EntityModel = entityModel;
            DiscoverCrawlerUrls = discoverCrawlerUrls;
            _CookieContainer = cookieContainer;
        }

        public WebCrawlerSourceNode(string[] rootUrls, XMLEntityModel entityModel, CookieContainer cookieContainer = null, params string[] urlPatterns)
            : this(rootUrls, entityModel, (content, url) => GetUrlsFromHtmlByPatterns(content, url, urlPatterns), cookieContainer)
        {   }

        protected override IEnumerable<Entity> GetEntities()
        {
            while (!_NextUrlQueue.IsEmpty)
            {
                string[] urls = _NextUrlQueue.ToArray();
                StringUtil.Shuffle(urls); // Make the crawler more like BFS
                
                _NextUrlQueue = new ConcurrentQueue<string>();
                ConcurrentQueue<Entity> resultEntities = new ConcurrentQueue<Entity>();
                Parallel.ForEach(urls,
                                new ParallelOptions()
                                {
                                    MaxDegreeOfParallelism = MaxParallelCount
                                },
                                (url) =>
                                {
                                    string content = HttpRequestHelper.GetContentFromHttpUrl(url);
                                    foreach (Entity entity in HtmlAgilityHelper.GetEntitiesFromContent(content, EntityModel))
                                    {
                                        entity.SetValue("Url", url);
                                        resultEntities.Enqueue(entity);
                                    }

                                    foreach (string nextUrl in DiscoverCrawlerUrls(content, url))
                                    {
                                        if (VisitedUrls.Contains(nextUrl)) continue;
                                        _NextUrlQueue.Enqueue(nextUrl);
                                        VisitedUrls.Add(nextUrl);
                                    }
                                });

                foreach (var entity in resultEntities)
                    yield return entity;
            }
        }

        private static IEnumerable<string> GetUrlsFromHtmlByPatterns(string content, string baseUrl, params string[] urlPatterns)
        {
            HtmlNode rootNode = HtmlAgilityHelper.ParseHtmlDocument(content);
            var aLinkNodes = rootNode.SelectNodes(".//a[@href]");
            var hrefRegexs = new List<Regex>();
            foreach (string urlPattern in urlPatterns)
            {
                hrefRegexs.Add(new Regex(urlPattern, RegexOptions.Compiled));
            }

            foreach (HtmlNode aLinkNode in aLinkNodes)
            {
                string href = aLinkNode.Attributes["href"].Value;
                if (href == null || href.Length == 0) continue;

                Uri result;
                if (Uri.TryCreate(new Uri(baseUrl), href, out result)){
                    href = result.AbsoluteUri;
                    foreach (Regex hrefRegex in hrefRegexs)
                    {
                        if (hrefRegex.IsMatch(href))
                        {
                            yield return href;
                            break;
                        }
                    }
                }
            }
        }
    }
}
