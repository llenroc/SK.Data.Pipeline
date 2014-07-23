using SK.RetryLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core.Common
{
    public static class HttpRequestHelper
    {
        public static string GetContentFromHttpUrl(string url, CookieContainer cookieContainer = null, ICredentials credentials = null, X509Certificate ceritficate = null)
        {
            return Retry.Func<string>(() =>
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.CookieContainer = cookieContainer ?? new CookieContainer();
                    if (credentials != null) request.Credentials = credentials;
                    if (ceritficate != null) request.ClientCertificates.Add(ceritficate);
                    if (url.StartsWith("https")) request.ProtocolVersion = HttpVersion.Version10;
                    request.UserAgent = @"SK.Data.Pipeline WebClient";

                    //Ignore all Certificates warning
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                    
                    HttpWebResponse response = (HttpWebResponse)(request.GetResponse());
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    return reader.ReadToEnd();
                }, 5, retryPolicy: new WebRetryPolicy(), intervalMilliSecond: 5000, waitType: RetryWaitType.Double);
        }
    }
}
