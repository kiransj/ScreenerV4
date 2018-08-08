using System;
using System.Collections.Generic;
using System.Linq;
using Helper;

namespace MarketData.StockDatabase
{
    public partial class StockDBApi
    {
        public DateTime GetLastUpdateDate()
        {
            return DayToDate(stockDatabase.EquityBhav.Select(x => x.Day).Max());
        }

        public List<EquityBhavTable> GetStockData(DateTime date)
        {
            int day = DateToDay(date);

            Globals.Log.Debug($"Querying database for stockData for date {date.ToString("dd-MMM-yyyy")}");
            var data = stockDatabase.EquityBhav.Where(x => x.Day == day).ToList();
            return data;
        }

        public List<EquityOHLCTable> GetOHLCData(DateTime date)
        {
            int day = DateToDay(date);

            Globals.Log.Debug($"Querying database for OHLCData for date {date.ToString("dd-MMM-yyyy")}");
            var data = stockDatabase.EquityOHLC.Where(x => x.Day == day).ToList();
            return data;
        }

        public List<HighLow52WeekTable> GetHighLow52Week()
        {
            Globals.Log.Debug($"Querying database for 52 week High low");
            var data = stockDatabase.HighLow52Week.ToList();
            return data;
        }

        public Dictionary<int, string> GetCompanyIdToSymbolMapping()
        {
            Globals.Log.Debug($"Querying database for companyId -> symbol mapping.");
            return stockDatabase.CompanyInformation.ToDictionary(x => x.CompanyId, x => x.Symbol);
        }

        public Dictionary<int, string> GetIndexIdToSymbolMapping()
        {
            Globals.Log.Debug($"Querying database for IndexID -> String mapping.");
            return stockDatabase.IndexInformation.ToDictionary(x => x.IndexId, x => x.IndexName);
        }

        public List<IndexBhavTable> GetIndexData(DateTime date)
        {
            int day = DateToDay(date);

            Globals.Log.Debug($"Querying database for Indexdata for date {date.ToString("dd-MMM-yyyy")}");
            return stockDatabase.IndexBhav.Where(x => x.Day == day).ToList();
        }
    }
}