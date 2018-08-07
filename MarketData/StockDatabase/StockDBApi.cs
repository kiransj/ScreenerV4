using System;
using System.Collections.Generic;
using System.Linq;
using Helper;
using MarketData.NseMarket;

namespace MarketData.StockDatabase
{
    public class StockDBApi
    {
        readonly DateTime FirstDay = new DateTime(2016, 1, 1);
        private StockDataContext stockDatabase;
        public StockDBApi()
        {
            stockDatabase = new StockDataContext();
        }


        private int DateToDay(DateTime date)
        {
            if(date.Year < 2016) {
                Globals.Log.Error($"Date {date} is less than 2016");
                throw new Exception($"Date {date} is less than 2016");
            }
            return (date - FirstDay).Days;
        }

        private DateTime DayToDate(int day)
        {
            return FirstDay.AddDays(day);
        }

        private EquityInformationTable EquityInformationOfUnknown(string symbol, string series, string IsinNumber)
        {
            EquityInformationTable et = new EquityInformationTable();
            et.Symbol = symbol;
            et.CompanyName = symbol;
            et.ISINNumber = IsinNumber;
            et.MarketLot = 1;
            et.PaidUpValue = 1;
            et.Series = "EQ";
            et.DateOfListing = FirstDay;
            et.FaceValue = 1;
            et.IsETF = false;
            et.Underlying = "";
            return et;
        }

        private EquityInformationTable EquityInformationToTable(EquityInformation e)
        {
            EquityInformationTable et = new EquityInformationTable();
            et.Symbol = e.Symbol;
            et.CompanyName = e.CompanyName;
            et.ISINNumber = e.IsinNumber;
            et.MarketLot = e.MarketLot;
            et.PaidUpValue = e.PaidUpValue;
            et.Series = e.Series;
            et.DateOfListing = e.DateOfListing;
            et.FaceValue = e.FaceValue;
            et.IsETF = false;
            et.Underlying = "";
            return et;
        }

        private EquityInformationTable ETFInformationToTable(ETFInformation e)
        {
            EquityInformationTable et = new EquityInformationTable();
            et.Symbol = e.Symbol;
            et.CompanyName = e.ETFName;
            et.ISINNumber = e.IsinNumber;
            et.MarketLot = e.MarketLot;
            et.DateOfListing = e.DateOfListing;
            et.FaceValue = e.FaceValue;
            et.IsETF = true;
            et.Underlying = e.Underlying;
            et.PaidUpValue = -1;
            et.Series = "ETF";
            return et;
        }

        private IndexBhavTable IndexBhavToTable(IndexBhav bhav, int indexId)
        {
            var indexBhav = new IndexBhavTable();

            indexBhav.IndexId = indexId;
            indexBhav.Day = DateToDay(bhav.IndexDate);
            indexBhav.Open = bhav.OpenValue;
            indexBhav.Close = bhav.CloseValue;
            indexBhav.High = bhav.HighValue;
            indexBhav.Low = bhav.LowValue;
            indexBhav.PointsChange = bhav.PointsChange;
            indexBhav.PointsChangePct = bhav.PointsChangePct;
            indexBhav.Volume = bhav.Volume;
            indexBhav.TurnOver = Math.Round(bhav.TurnOver/100000, 2);
            indexBhav.PE = bhav.PE;
            indexBhav.PB = bhav.PB;
            indexBhav.DivYield = bhav.DivYield;
            Globals.Log.Debug($"Adding Index {indexBhav.IndexId}/{indexBhav.Day}");
            return indexBhav;
        }

        private EquityBhavTable EquityBhavToTable(Bhav bhav, int companyId, long deliveredQty)
        {
            var equityBhav = new EquityBhavTable();

            equityBhav.CompanyId = companyId;
            equityBhav.Day = DateToDay(bhav.TimeStamp);
            equityBhav.Close = bhav.Close;
            equityBhav.PrevClose = bhav.PrevClose;
            equityBhav.TotalTradedQty = bhav.TotTradedQty;
            equityBhav.TotalTradedValue = Math.Round(bhav.TotTradedValue/100000, 2);
            equityBhav.TotalTrades = bhav.TotalTrades;
            equityBhav.TotalDeliveredQty = deliveredQty == -1 ? bhav.TotTradedQty : deliveredQty;
            return equityBhav;
        }

        private EquityOHLCTable EquityOHLCToTable(Bhav bhav, int companyId)
        {
            var ohlc = new EquityOHLCTable();
            ohlc.CompanyId = companyId;
            ohlc.Day = DateToDay(bhav.TimeStamp);
            ohlc.Close = bhav.Close;
            ohlc.High = bhav.High;
            ohlc.Last = bhav.Last;
            ohlc.Low = bhav.Low;
            ohlc.Open = bhav.Open;
            ohlc.PrevClose = bhav.PrevClose;

            return ohlc;
        }

        public int AddBhavData(List<Bhav> bhav, List<DeliveryPosition> deliveryPosition, List<IndexBhav> indexBhav)
        {
            Globals.Log.Info($"Updating Bhav/IndexBhav for date {bhav[0].TimeStamp} to DB");
            var mapping = stockDatabase.CompanyInformation.ToDictionary(x => x.Symbol, x => x.CompanyId);
            var dp_mapping = deliveryPosition.Where(x => x.Series == "EQ" || x.Series == "BE").ToDictionary(x => x.Symbol, x => x);

            // Find new companies and add it to DB
            foreach(var item in bhav.Where(x => x.Series == "EQ" || x.Series == "BE"))
            {
                if(!mapping.ContainsKey(item.Symbol))
                {
                    Globals.Log.Info($"Adding unknown company {item.Symbol}");
                    stockDatabase.CompanyInformation.Add(EquityInformationOfUnknown(item.Symbol, item.Series, item.ISINNumber));
                }
            }
            stockDatabase.SaveChanges();

            // Add the bhav data to DB
            mapping = stockDatabase.CompanyInformation.ToDictionary(x => x.Symbol, x => x.CompanyId);
            foreach(var item in bhav.Where(x => x.Series == "EQ" || x.Series == "BE"))
            {
                long deliveredQty = dp_mapping.ContainsKey(item.Symbol) ? dp_mapping[item.Symbol].DeliverableQty : -1;
                stockDatabase.EquityBhav.Add(EquityBhavToTable(item, mapping[item.Symbol], deliveredQty));
                stockDatabase.EquityOHLC.Add(EquityOHLCToTable(item, mapping[item.Symbol]));
            }

            var indexMapping = stockDatabase.IndexInformation.ToDictionary(x => x.IndexName, x => x.IndexId);
            foreach(var item in indexBhav.OrderBy(x => x.IndexName))
            {
                if(indexMapping.ContainsKey(item.IndexName))
                    stockDatabase.IndexBhav.Add(IndexBhavToTable(item, indexMapping[item.IndexName]));
            }

            return stockDatabase.SaveChanges();
        }

        public int AddOrUpdateEquityInformation(List<EquityInformation> equitys, List<ETFInformation> etfs, List<IndexInformation> indexes)
        {
            Globals.Log.Info($"Updating/Adding Company/Index/ETF to DB");
            // Remove any duplicate entries
            var mappingEquity = equitys.GroupBy(x => x.Symbol)
                                       .Distinct()
                                       .Select(x => x.First())
                                       .ToDictionary(x => x.Symbol, x => x);

            var mappingEtf = etfs.GroupBy(x => x.Symbol)
                                       .Distinct()
                                       .Select(x => x.FirstOrDefault())
                                       .ToDictionary(x => x.Symbol, x => x);

            foreach (var item in stockDatabase.CompanyInformation)
            {
                if (mappingEquity.ContainsKey(item.Symbol))
                {
                    var e = mappingEquity[item.Symbol];
                    item.Symbol = e.Symbol;
                    item.CompanyName = e.CompanyName;
                    item.ISINNumber = e.IsinNumber;
                    item.MarketLot = e.MarketLot;
                    item.PaidUpValue = e.PaidUpValue;
                    item.Series = e.Series;
                    item.DateOfListing = e.DateOfListing;
                    item.FaceValue = e.FaceValue;
                    item.IsETF = false;
                    item.Underlying = "";

                    // Remove the symbol from mapping
                    mappingEquity.Remove(item.Symbol);
                }
                else if (mappingEtf.ContainsKey(item.Symbol))
                {
                    var e = mappingEtf[item.Symbol];
                    item.Symbol = e.Symbol;
                    item.CompanyName = e.ETFName;
                    item.ISINNumber = e.IsinNumber;
                    item.MarketLot = e.MarketLot;
                    item.DateOfListing = e.DateOfListing;
                    item.FaceValue = e.FaceValue;
                    item.IsETF = true;
                    item.Underlying = e.Underlying;
                    item.PaidUpValue = -1;
                    item.Series = "ETF";

                    // Remove the symbol from mapping
                    mappingEtf.Remove(item.Symbol);
                }
            }

            // If there are any new company or ETF lets update it.
            foreach(var company in mappingEquity)
            {
                Globals.Log.Info($"Adding Company '{company.Value.Symbol}' to DB");
                stockDatabase.CompanyInformation.Add(EquityInformationToTable(company.Value));
            }

            foreach(var etf in mappingEtf)
            {
                Globals.Log.Info($"Adding ETF '{etf.Value.Symbol}' to DB");
                stockDatabase.CompanyInformation.Add(ETFInformationToTable(etf.Value));
            }

            var mappingIndex = stockDatabase.IndexInformation.ToDictionary(x => x.IndexName, x => x);
            foreach(var item in indexes)
            {
                if(!mappingIndex.ContainsKey(item.IndexName))
                {
                    Globals.Log.Info($"Adding Index '{item.IndexName}' to DB");
                    stockDatabase.IndexInformation.Add(new IndexInformationTable() { IndexName = item.IndexName});
                }
            }

            return stockDatabase.SaveChanges();
        }
    }
}