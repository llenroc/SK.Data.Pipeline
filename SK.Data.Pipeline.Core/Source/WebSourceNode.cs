using SK.RetryLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class WebSourceNode : SourceNode
    {
        public string Url { set; get; }
        public ICredentials Credentials { get; set; }

        public WebSourceNode(string url, ICredentials credentials = null)
            : base()
        {
            Url = url;
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            string content = Retry.Func<string>(() =>
            {
                WebRequest request = WebRequest.Create(Url);
                if (Credentials != null)
                    request.Credentials = Credentials;

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                return reader.ReadToEnd();
            }, 5, retryPolicy: new WebRetryPolicy(), intervalMilliSecond: 5000, waitType: RetryWaitType.Double);

            var entity = new Entity();
            entity.SetValue(Entity.DefaultColumn, content);

            yield return entity;
        }
    }
}
