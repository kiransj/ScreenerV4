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
            wc.Headers["User-Agent"] = userAgent ?? Globals.options.httpOptions.UserAgent;
            wc.Headers["Host"] = host ?? Globals.options.httpOptions.Host;
        }

        public void DownloadAsString(string url, out string data)
        {
            Globals.log.Info($"url -> string: {url}");
            data = wc.DownloadString(url);
            //await wc.DownloadStringAsync(new Uri(url, UriKind.Absolute));
        }

        public void DownloadAsFile(string url, string filename)
        {
            Globals.log.Info($"url -> file <{filename}>: {url}");
            wc.DownloadFile(url, filename);
        }
    }
}