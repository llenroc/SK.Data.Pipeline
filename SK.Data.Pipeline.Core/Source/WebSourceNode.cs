using SK.Data.Pipeline.Core.Common;
using SK.RetryLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class WebSourceNode : SourceNode
    {
        public string Url { set; get; }
        public ICredentials Credentials { get; set; }
        public CookieContainer CookieContainer { get; set; }

        public X509Certificate Ceritficate { get; set; }

        public WebSourceNode(string url, CookieContainer cookieContainer = null, ICredentials credentials = null, X509Certificate ceritficate = null)
            : base()
        {
            Url = url;
            Credentials = credentials;
            CookieContainer = cookieContainer;
            Ceritficate = ceritficate;
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            string content = HttpRequestHelper.GetContentFromHttpUrl(Url, CookieContainer, Credentials, Ceritficate);

            var entity = new Entity();
            entity.SetValue(Entity.DefaultColumn, content);

            yield return entity;
        }
    }
}
