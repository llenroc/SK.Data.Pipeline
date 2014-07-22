using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SK.Data.Pipeline.Core;
using SK.Data.Pipeline;
using System.Net;
using System.IO;

namespace SK.Data.Pipeline.Core.Test
{
    [TestClass]
    public class WebTest
    {
        const string Province = "Province";
        const string Output = "Output";

        [TestMethod]
        public void HtmlBasic()
        {
            XMLEntityModel model = new XMLEntityModel(@"//table[@class='wikitable sortable']/tr[not(@*)]");
            model.AddXMLColumn("GB", "./td[1]");
            model.AddXMLColumn("Province", "./td[3]");

            PipelineTask.FromWeb("http://en.wikipedia.org/wiki/China_provinces")
                        .ParseHtml(model)
                        .AddMonitor((entity) =>
                        {
                            Console.WriteLine();
                        })
                        .ToFile(Output, model)
                        .Start();

            TestHelper.CompareTwoFile(Province, Output);
        }

        [TestMethod]
        public void CrawlerTest()
        {
            CookieContainer cookieContainer = new CookieContainer();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"https://bbs.sjtu.edu.cn/bbslogin?id=guest");
            request.ProtocolVersion = HttpVersion.Version10;
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = @"Mozilla/5.0";
            HttpWebResponse response = (HttpWebResponse)(request.GetResponse());
            cookieContainer.Add(response.Cookies);

            string url = @"https://bbs.sjtu.edu.cn/bbsdoc?board=PPPerson";
            var model = new XMLEntityModel(@".//tr[position() > 1]");
            model.AddXMLColumn("ID", @"./td[1]");

            WebCrawlerSourceNode crawler = new WebCrawlerSourceNode(new string[] { url }, model, cookieContainer, @"bbs.sjtu.edu.cn");
            PipelineTask.Create(crawler)
                         .AddMonitor(
                         (entity) =>
                         {
                             Console.WriteLine(entity);
                         })
                         .Start();
        }
    }
}
