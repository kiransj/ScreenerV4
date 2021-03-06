using System;
using System.IO;
using Newtonsoft.Json;

namespace Helper
{

    public class HttpServer
    {
        public int Port { get; set; }
        public HttpServer() => Port = 6001;
    }
    public class HttpClientOptions
    {
        public string UserAgent { get; set; }
        public string Host { get; set; }

        public HttpClientOptions()
        {
            UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Mobile Safari/537.36";
            Host = "www.nseindia.com";
        }
    }

    public class DbOptions
    {
        public string DBFolder { get; set; }
        public string DBFile { get; set; }
        public DbOptions()
        {
            DBFolder = null;
            DBFile = null;
        }
    }

    public class AppOptions
    {
        public bool CachingEnabled { get; set; } // Enable Caching
        public string TmpFolder { get; set; } //Folder to unzip files
        public string LogFileName { get; set; }
        public string marketCapFile { get; set; }
        public HttpClientOptions httpClient;
        public HttpServer httpServer;
        public DbOptions dbOptions;

        public AppOptions()
        {
            CachingEnabled = false;
            TmpFolder = null;
            httpClient = new HttpClientOptions();
            dbOptions = new DbOptions();
            httpServer = new HttpServer();
        }

        public override string ToString()
        {
            string output = "{\n";
            output += $"\t\tCachingEnabled: {CachingEnabled}\n";
            output += $"\t\tTmpFolder: {TmpFolder}\n";
            output += $"\t\tLogFileName: {LogFileName}\n";
            output += "\t\thttpOptions: {\n";
            output += $"\t\t\tUserAgent: {httpClient.UserAgent}\n";
            output += $"\t\t\tHost: {httpClient.Host}\n";
            output += "\t\t}\n";
            output += "\t}\n";
            return output;
        }
    }

    public class Options
    {
        private static readonly object padlock = new object();
        private static AppOptions options = null;
        private static string fileName = "";
        private static Logger log = Logger.GetLoggerInstance();
        static public AppOptions app { get { return options; } }
        Options()
        {

        }

        static public void SetOptions(string filename)
        {
            if (options == null)
            {
                lock (padlock)
                {
                    if (options == null)
                    {
                        options = JsonConvert.DeserializeObject<AppOptions>(File.ReadAllText(filename));
                        Options.fileName = filename;
                        log.Info($"Options file {filename}");
                    }
                }
            }
            else
            {
                log.Error($"Trying to Initialize options twice. Options already initialized with contents of file {fileName}");
                throw new Exception($"Trying to Initialize options twice. Options initialized with contents of fiel {fileName}");
            }
        }
    }
}