using System;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using System.IO;

namespace Helper
{
    public class FileDownloader
    {

        private string userAgent;
        private string host;
        public FileDownloader(string userAgent = null, string host = null)
        {
            this.userAgent = userAgent ?? Globals.Options.httpClient.UserAgent;
            this.host = host ?? Globals.Options.httpClient.Host;
        }

        public void DownloadAsString(string url, out string data)
        {
            Globals.Log.Info($"url -> string: {url}");
            WebClient wc = new WebClient();
            Stopwatch sw = new Stopwatch();

            wc.Headers["User-Agent"] = userAgent;
            wc.Headers["Host"] = host;
            sw.Start();
            data = wc.DownloadString(url);
            sw.Stop();
            Globals.Log.Info($"{url} took {sw.Elapsed} seconds");
        }

        public void DownloadAsFile(string url, string filename)
        {
            Stopwatch sw = new Stopwatch();
            Globals.Log.Info($"url -> file <{filename}>: {url}");

            WebClient wc = new WebClient();
            wc.Headers["User-Agent"] = userAgent;
            wc.Headers["Host"] = host;
            sw.Start();
            wc.DownloadFile(url, filename);
            sw.Stop();
            Globals.Log.Info($"{url} took {sw.Elapsed} seconds");
        }

        public async Task Download(string url, string filename)
        {
            Stopwatch sw = new Stopwatch();
            Globals.Log.Info($"url -> file (async) <{filename}>: <{url}>");
            sw.Start();
            using (HttpClient client = new HttpClient())
            {
                // Set the HTTP headers
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);
                client.DefaultRequestHeaders.Add("Host", host);
                using (HttpResponseMessage response = await client.GetAsync(url))
                using (HttpContent content = response.Content)
                {
                    // Read the data
                    var result = await content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(filename, result);
                }
            }
            sw.Stop();
            Globals.Log.Info($"{url} (async) took {sw.Elapsed} seconds");
            return;
        }
    }
}