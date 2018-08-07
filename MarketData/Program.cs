using System;
using System.Diagnostics;
using Helper;
using MarketData.NseMarket;
namespace MarketData
{
    class Program
    {
        static Logger log = Logger.GetLoggerInstance();

        void DownloadTodayData()
        {
            MarketAPI api = new MarketAPI();
            var task = api.GetDailyData(DateTime.Now.AddDays(-1));
            task.Wait();
            Globals.Log.Info(task.Result.ToString());
            return;
        }
        static void Main(string[] args)
        {
            Globals.InitGlobals("Options.json");
            //string data;
            //fileDownloader.DownloadAsString(url.ETFListUrl, out data);
            //Globals.Log.Info(data);

            Program p = new Program();
            p.DownloadTodayData();
        }
    }
}
