using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CollectorCore.Utils
{
    public class HttpUtils
    {
        public static async Task<string> RedirectPathAsync(string url)
        {
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            }

            string location = string.Empty;
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.AllowAutoRedirect = false;
            try
            {
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.Redirect)
                    {
                        location = response.GetResponseHeader("Location");
                    }
                    else if(response.StatusCode == HttpStatusCode.OK)
                    {
                        location = url;
                    }
                }
            }
            catch{}
            return location;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;   
        }

        private static StringBuilder builder = new StringBuilder();
        public static string GetHostByUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            builder.Clear();

            if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = url.TrimStart("https://".ToCharArray());
                builder.Append("https://");
            }
            else if(url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                url = url.TrimStart("http://".ToCharArray());
                builder.Append("http://");
            }
            else
            {
                return string.Empty;
            }

            url = url.TrimStart('/');
            url = url.Split('/')[0];

            builder.Append("www.");
            var list = url.Split('.');
            if (list.Length < 2)
                return string.Empty;

            var result = builder.Append($"{list[list.Length - 2]}.{list[list.Length - 1]}").ToString();
            return result;
        }
    }
}
