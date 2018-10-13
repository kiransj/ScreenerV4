
using System;

using MarketData.NseMarket;
using MarketData.StockDatabase;
using Helper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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

        public async Task<int> UpdateStockDataFor(DateTime dateToUpdate)
        {
            int count = 0;
            var data = await marketApi.GetDailyData(dateToUpdate);
            if(data != null)
            {
                //dbApi.AddOrUpdateEquityInformation(data.Equitys, data.Etfs, data.Indexes);
                count += dbApi.AddBhavData(dateToUpdate, data.BhavData, data.deliveryPosition, data.IndexBhavData,
                                                            data.circuitBreaker, data.highLow52Week, data.niftyOptionBhav);

            }
            else
            {
                Globals.Log.Error($"Failed to Update Market data for date {dateToUpdate}");
            }
            return count;
        }

        public async Task<int> UpdateNiftyIndexToToday()
        {
            var updatedDate = dbApi.GetTradedDaysN();
            int updatedDays = 0;
            for(int i = 0; i < updatedDate.Count(); i++)
            {
                var dateToUpdate = updatedDate[i];
                Globals.Log.Info($"Updating Market data for date {dateToUpdate}");

                var data = await marketApi.GetDailyData(dateToUpdate);
                if(data != null)
                {
                    int count = dbApi.AddNiftyIndexOptionsData(dateToUpdate, data.IndexBhavData);
                    Globals.Log.Info($"Updated Nifty INdex data for date {dateToUpdate.ToShortDateString()}");

                    updatedDays++;
                }
                else
                {
                    Globals.Log.Error($"Failed to Update Market data for date {dateToUpdate}");
                }
            }
            return updatedDays;
        }


        public async Task<int> UpdateNiftyOptionsToToday()
        {
            var updatedDate = dbApi.GetTradedDaysN();
            int updatedDays = 0;
            for(int i = 0; i < updatedDate.Count(); i++)
            {
                var dateToUpdate = updatedDate[i];
                Globals.Log.Info($"Updating Market data for date {dateToUpdate}");

                var data = await marketApi.GetDailyData(dateToUpdate);
                if(data != null)
                {
                    dbApi.AddNiftyStockOptionsData(dateToUpdate, data.niftyOptionBhav);
                    /*dbApi.AddBhavData(dateToUpdate, data.BhavData, data.deliveryPosition, data.IndexBhavData,
                                                    data.circuitBreaker, data.highLow52Week, data.niftyOptionBhav);*/

                    updatedDays++;
                }
                else
                {
                    Globals.Log.Error($"Failed to Update Market data for date {dateToUpdate}");
                }
            }
            return updatedDays;
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
                    //dbApi.AddOrUpdateEquityInformation(data.Equitys, data.Etfs, data.Indexes);
                    //dbApi.AddNiftyStockOptionsData(dateToUpdate, data.niftyOptionBhav);
                    dbApi.AddBhavData(dateToUpdate, data.BhavData, data.deliveryPosition, data.IndexBhavData,
                                                             data.circuitBreaker, data.highLow52Week, data.niftyOptionBhav);

                    updatedDays++;
                }
                else
                {
                    Globals.Log.Error($"Failed to Update Market data for date {dateToUpdate}");
                }
            }
            return updatedDays;
        }

        public Dictionary<int, Int64> GetMarketCap()
        {
            var list = marketApi.GetMarketCap();
            var mapping = dbApi.GetIsinToCompanyIdMapping();

            var result = new Dictionary<int, Int64>();
            foreach(var item in list)
            {
                if(mapping.ContainsKey(item.IsinNumber))
                {
                    int companyId = mapping[item.IsinNumber];
                    if(!result.ContainsKey(companyId))
                    {
                        result[companyId] = item.numOfShares;
                    }
                    else
                    {
                        Globals.Log.Error($"ISIN number {item.IsinNumber}, companyId: {companyId} appears twice in marketCap file");
                    }
                }
            }
            return result;
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

        public List<NiftyBhavTable> GetNiftyOptionsData(DateTime date)
        {
            var result = dbApi.GetNiftyOptionsData(date);
            return result;
        }

        public List<DateTime> GetTradedDates()
        {
            return dbApi.GetTradedDays();
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


        public int AddStockToFavList(string symbol, string listName)
        {
            try
            {
                return dbApi.AddToFavList(symbol, listName);
            }
            catch(Exception ex)
            {
                Globals.Log.Error($"Unable to add '{symbol}' to favList '{listName}' due to error `{ex.Message}`");
                return 0;
            }
        }

        public List<NiftyBhavTable> GetNiftyOptionsDataFor(DateTime expDate, long strikePrice, bool callOptions)
        {
            return dbApi.GetNiftyOptionsDataFor(expDate, strikePrice, callOptions);
        }


        public int RemoveStockToFavList(string symbol, string listName)
        {
            try
            {
                return dbApi.RemoveFromFavList(symbol, listName);
            }
            catch(Exception ex)
            {
                Globals.Log.Error($"Unable to remove '{symbol}' from favList '{listName}' due to error `{ex.Message}`");
                return 0;
            }
        }

        public List<StockFavList> GetStockFavList()
        {
            var list = dbApi.GetListOfEquity().Where(x => x.IsETF).Select(x => new StockFavList() {ListName = "ETF", Symbol = x.Symbol});
            var result = dbApi.GetFavList();
            result.AddRange(list);
            return result;
        }
    }
}