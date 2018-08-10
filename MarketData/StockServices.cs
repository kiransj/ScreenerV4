
using System;

using MarketData.NseMarket;
using MarketData.StockDatabase;
using Helper;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MarketData
{
    public class StockServices
    {
        private MarketAPI marketApi = new MarketAPI();
        private StockDBApi dbApi = new StockDBApi();

        public StockServices() { }

        public DateTime GetLastUpdatedDate()
        {
            return dbApi.GetLastUpdateDate();
        }

        public async Task<int> UpdateStockDataToToday()
        {
            var updatedDate = dbApi.GetLastUpdateDate();
            int daysToUpdate = (DateTime.Now - updatedDate).Days;
            int updatedDays = 0;
            Globals.Log.Info($"Last Updated on {updatedDate.ToString("dd-MMM-yyyy")}. Need to update for {daysToUpdate} days");
            for(int i = 1; i <= daysToUpdate; i++)
            {
                var dateToUpdate = updatedDate.AddDays(i);
                Globals.Log.Info($"Updating Market data for date {dateToUpdate}");

                var data = await marketApi.GetDailyData(dateToUpdate);
                if(data != null)
                {
                    dbApi.AddOrUpdateEquityInformation(data.Equitys, data.Etfs, data.Indexes);
                    dbApi.AddBhavData(dateToUpdate, data.BhavData, data.deliveryPosition, data.IndexBhavData,
                                                             data.circuitBreaker, data.highLow52Week);
                    updatedDays++;
                }
                else
                {
                    Globals.Log.Error($"Failed to Update Market data for date {dateToUpdate}");
                }
            }
            return updatedDays;
        }

        public (List<EquityInformationTable> Companies, List<IndexInformationTable> Indexes) GetListOfEquityIndex()
        {
            var company = dbApi.GetListOfEquity();
            var index = dbApi.GetListOfIndex();
            return (company, index);
        }

        public (List<EquityBhavTable> bhav, List<EquityOHLCTable> ohlc, List<HighLow52WeekTable> highLow) GetStockReport(DateTime date)
        {
            var result1 = dbApi.GetStockData(date);
            var result2 = dbApi.GetOHLCData(date);
            var result3 = dbApi.GetHighLow52Week();
            return (result1, result2, result3);
        }

        public List<EquityBhavTable> GetStockHistory(string symbol)
        {
            return dbApi.GetHistory(symbol);
        }

        public DateTime DayToDate(int day)
        {
            return dbApi.DayToDate(day);
        }

        public Dictionary<int, string> GetSymbolToCompanyId()
        {
            return dbApi.GetCompanyIdToSymbolMapping();
        }


        public Dictionary<int, string> GetCompanyIdToSymbol()
        {
            return dbApi.GetCompanyIdToSymbolMapping();
        }
    }
}