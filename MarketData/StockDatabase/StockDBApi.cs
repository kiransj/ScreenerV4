using System.Collections.Generic;
using System.Linq;
using MarketData.NseMarket;

namespace MarketData.StockDatabase
{
    public class StockDBApi
    {
        private StockDataContext stockDatabase;
        public StockDBApi()
        {
            stockDatabase = new StockDataContext();
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

        public void AddOrUpdateEquityInformation(List<EquityInformation> equitys, List<ETFInformation> etfs)
        {
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

            foreach(var company in mappingEquity)
            {
                stockDatabase.CompanyInformation.Add(EquityInformationToTable(company.Value));
            }

            foreach(var etf in mappingEtf)
            {
                stockDatabase.CompanyInformation.Add(ETFInformationToTable(etf.Value));
            }
            stockDatabase.SaveChanges();
        }
    }
}