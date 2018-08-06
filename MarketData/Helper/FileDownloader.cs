using System;
using System.Net;
using System.Threading.Tasks;

namespace Helper
{
    public class FileDownloader
    {
        WebClient wc;
        public FileDownloader(string userAgent = null, string host = null)
        {
            wc = new WebClient();
            wc.Headers["User-Agent"] = userAgent ?? Globals.Options.httpClient.UserAgent;
            wc.Headers["Host"] = host ?? Globals.Options.httpClient.Host;
        }

        public void DownloadAsString(string url, out string data)
        {
            Globals.Log.Info($"url -> string: {url}");
            data = wc.DownloadString(url);
            //await wc.DownloadStringAsync(new Uri(url, UriKind.Absolute));
        }

        public void DownloadAsFile(string url, string filename)
        {
            Globals.Log.Info($"url -> file <{filename}>: {url}");
            wc.DownloadFile(url, filename);
        }
    }
}