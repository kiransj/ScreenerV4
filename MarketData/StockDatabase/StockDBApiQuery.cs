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

        public List<DateTime> GetTradedDaysN() {
            var result = stockDatabase.EquityBhav.Select(x => x.Day)
                                                 .OrderBy(x => x)
                                                 .GroupBy(x => x)
                                                 .Select(x => x.Key)
                                                 .Select(x => DayToDate(x))
                                                 .ToList();

            return result;
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

        public List<NiftyBhavTable> GetNiftyOptionsData(DateTime date)
        {
            int day = DateToDay(date);
            Globals.Log.Debug($"Querying database for niftyOptions data for date {date.ToString("dd-MMM-yyyy")}");
            var data = stockDatabase.NiftyBhav.Where(x => x.Day == day).ToList();
            return data;
        }

        public List<NiftyBhavTable> GetNiftyOptionsDataFor(DateTime expDate, long strikePrice, bool callOptions)
        {
            int day = DateToDay(expDate);
            Globals.Log.Debug($"Querying database for niftyOptions data for {expDate.ToString("dd-MMM-yyyy")} and strike price {strikePrice} and callOptions : {callOptions.ToString()}");

            var data = stockDatabase.NiftyBhav.Where(x => (x.ExpDay == day && x.StrikePrice == strikePrice && x.CallOption == callOptions)) .ToList();

            return data;
        }

        public List<HighLow52WeekTable> GetHighLow52Week()
        {
            Globals.Log.Debug($"Querying database for 52 week High low");
            var data = stockDatabase.HighLow52Week.ToList();
            return data;
        }

        public Dictionary<string, int> GetIsinToCompanyIdMapping()
        {
            Globals.Log.Debug($"Querying database for IsinNumber -> companyId mapping.");
            return stockDatabase.CompanyInformation.ToDictionary(x => x.ISINNumber, x => x.CompanyId);
        }

        public Dictionary<int, string> GetCompanyIdToSymbolMapping()
        {
            Globals.Log.Debug($"Querying database for companyId -> symbol mapping.");
            return stockDatabase.CompanyInformation.ToDictionary(x => x.CompanyId, x => x.Symbol);
        }

        public List<DateTime> GetTradedDays()
        {
            Globals.Log.Debug($"Querying database for traded days");
            return stockDatabase.EquityBhav.Select(x => DayToDate(x.Day)).OrderByDescending(x => x).Distinct().ToList();
        }


        public List<EquityBhavTable> GetHistory(string symbol)
        {
            try
            {
                Globals.Log.Debug($"Querying database for history of {symbol}");
                var companyId = stockDatabase.CompanyInformation.Where(x => x.Symbol == symbol).Select(x => x.CompanyId).First();
                return stockDatabase.EquityBhav.Where(x => x.CompanyId == companyId).OrderByDescending(x => x.Day).ToList();
            }
            catch(Exception)
            {
                Globals.Log.Error($"Querying database for history of {symbol} failed as symbol not found.");
                return new List<EquityBhavTable>();
            }
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

        public List<EquityInformationTable> GetListOfEquity()
        {
            Globals.Log.Debug($"Querying database for List of companies");
            return stockDatabase.CompanyInformation.ToList();
        }

        public List<IndexInformationTable> GetListOfIndex()
        {
            Globals.Log.Debug($"Querying database for List of Index");
            return stockDatabase.IndexInformation.ToList();
        }

        public List<StockFavList> GetFavList()
        {
            Globals.Log.Debug($"Querying database for FavList");
            return stockDatabase.Favlist.ToList();
        }
    }
}