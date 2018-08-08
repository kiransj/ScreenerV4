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

        NseDailyData DownloadTodayData(DateTime date)
        {
            MarketAPI api = new MarketAPI();
            var task = api.GetDailyData(date);
            task.Wait();
            if(task.Result != null)
                Globals.Log.Info(task.Result.ToString());
            return task.Result;
        }

        void StockDB()
        {
            StockDBApi api = new StockDBApi();
            var date = new DateTime(2018, 01, 31);
            for(int i = 0; i < 360; i++)
            {
                if(i >= DateTime.Now.DayOfYear)
                    break;

                var date1 = date.AddDays(i);
                if(!(date1.DayOfWeek == DayOfWeek.Saturday || date1.DayOfWeek == DayOfWeek.Sunday))
                {
                    Globals.Log.Error($"Downloading data for {date1} {i}");
                    var data = DownloadTodayData(date1);
                    if(data != null)
                    {
                        int count = api.AddOrUpdateEquityInformation(data.Equitys, data.Etfs, data.Indexes);
                        count += api.AddBhavData(date1, data.BhavData, data.deliveryPosition,
                                                                            data.IndexBhavData, data.circuitBreaker,
                                                                            data.highLow52Week);
                        Globals.Log.Info($"Updated {count} rows");
                    }
                    else
                    {
                        Globals.Log.Error($"Data does not exists for {date1}");
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            Globals.InitGlobals("Options.json");
            StockServices ss = new StockServices();
            var t = ss.UpdateStockDataToToday();
            t.Wait();
            Globals.Log.Info($"Updated data for {t.Result} days");
        }
    }
}
