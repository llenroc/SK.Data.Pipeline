using SK.RetryLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core.Common
{
    public static class HttpRequestHelper
    {
        public static string GetContentFromHttpUrl(string url, CookieContainer cookieContainer = null, ICredentials Credentials = null)
        {
            return Retry.Func<string>(() =>
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.CookieContainer = cookieContainer;
                    if (url.StartsWith("https")) 
                        request.ProtocolVersion = HttpVersion.Version10;
                    request.UserAgent = @"Mozilla/5.0";

                    if (Credentials != null)
                        request.Credentials = Credentials;

                    HttpWebResponse response = (HttpWebResponse)(request.GetResponse());
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    return reader.ReadToEnd();
                }, 5, retryPolicy: new WebRetryPolicy(), intervalMilliSecond: 5000, waitType: RetryWaitType.Double);
        }
    }
}
