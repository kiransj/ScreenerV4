using System;
using System.Diagnostics;
using System.Linq;
using Helper;
using MarketData.NseMarket;
using MarketData.StockDatabase;
namespace MarketData
{
    class Program
    {
        static Logger log = Logger.GetLoggerInstance();

        NseDailyData DownloadTodayData()
        {
            MarketAPI api = new MarketAPI();
            var task = api.GetDailyData(DateTime.Now.AddDays(-1));
            task.Wait();
            Globals.Log.Info(task.Result.ToString());
            return task.Result;
        }

        void StockDB()
        {
            StockDBApi api = new StockDBApi();
            var data = DownloadTodayData();

            api.AddOrUpdateEquityInformation(data.Equitys, data.Etfs);
        }


        static void Main(string[] args)
        {
            Globals.InitGlobals("Options.json");
            Program p = new Program();
            p.StockDB();
        }
    }
}
