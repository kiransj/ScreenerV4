using System;
using System.Collections.Generic;
using System.Linq;
using MarketData;
using MarketData.StockDatabase;

namespace Vue.Controllers
{
    public class CompanyInformation
    {
        public string companyName;
        public string symbol;
        public string IsinNumber;
        public DateTime DateOfListing;
    }

    public class ETFInformation
    {
        public string etfName;
        public string symbol;
        public string Underlying;
        public DateTime DateOfListing;
    }

    public class ListOfCompanyIndex
    {
        public List<CompanyInformation> CompanyList;
        public List<ETFInformation> EtfList;
        public List<string> IndexList;
        public ListOfCompanyIndex(List<EquityInformationTable> company, List<IndexInformationTable> index)
        {
            CompanyList = company.Where(x => x.IsETF == false).Select(x => new CompanyInformation()
                {
                    companyName = x.CompanyName,
                    symbol = x.Symbol,
                    IsinNumber = x.ISINNumber,
                    DateOfListing = x.DateOfListing
                }).ToList();
            EtfList = company.Where(x => x.IsETF == true).Select(x => new ETFInformation()
                {
                    etfName = x.CompanyName,
                    symbol = x.Symbol,
                    Underlying = x.Underlying,
                    DateOfListing = x.DateOfListing
                }).ToList();

            IndexList = index.Select(x => x.IndexName).ToList();
        }
    }
    public class StockDailyReport
    {
        public string symbol;
        public double close;
        public double change;
        public double totQty;
        public double totTrades;
        public double totTraVal;
        public double totDelQty;
        public string circuitBreaker;
        public string upDown;
        public double high52week;
        public double low52week;

        public StockDailyReport()
        {
            high52week = -1;
            low52week = -1;
            upDown = "";
            circuitBreaker = "";
        }
    }

    public class StockHistory
    {
        public DateTime date;
        public double close;
        public double change;
        public double totQty;
        public double totTrades;
        public double totTraVal;
        public double totDelQty;
    }

    public class StockReport
    {
        static List<EquityBhavTable> bhav;
        static List<EquityOHLCTable> ohlc;
        static List<HighLow52WeekTable> highLow;
        static Dictionary<int, string> mapping;

        static public List<StockHistory> GetStockHistory(string symbol)
        {
            StockServices stockService = new StockServices();
            var result = stockService.GetStockHistory(symbol);

            var history = result.Select(item => new StockHistory() {
                                   change = Math.Round(100.0 * (item.Close - item.PrevClose)/item.PrevClose, 2),
                                   date = stockService.DayToDate(item.Day),
                                   close = item.Close,
                                   totDelQty = item.TotalDeliveredQty,
                                   totTrades = item.TotalTrades,
                                   totTraVal = item.TotalTradedValue,
                                   totQty = item.TotalTradedQty
                                }).ToList();
            return history;
        }

        static public List<StockDailyReport> GetStockReport(DateTime date)
        {
            StockServices stockService = new StockServices();
            (bhav, ohlc, highLow) = stockService.GetStockReport(date);
            Dictionary<int, string> circuitBreaker = ohlc.Select(x => new { x.CompanyId, x.HighLow}).ToDictionary(x => x.CompanyId, x => x.HighLow);
            Dictionary<int, HighLow52WeekTable> hl = highLow.ToDictionary(x => x.CompanyId, x => x);
            mapping = stockService.GetCompanyIdToSymbol();
            List<StockDailyReport> report = new List<StockDailyReport>();

            foreach(var item in bhav)
            {
                if(mapping.ContainsKey(item.CompanyId))
                {
                    StockDailyReport sr = new StockDailyReport();
                    sr.symbol = mapping[item.CompanyId];
                    sr.totDelQty = item.TotalDeliveredQty;
                    sr.totQty = item.TotalTradedQty;
                    sr.totTrades = item.TotalTrades;
                    sr.totTraVal = item.TotalTradedValue;
                    sr.circuitBreaker = circuitBreaker.ContainsKey(item.CompanyId) ? circuitBreaker[item.CompanyId]: "";
                    sr.close = item.Close;
                    if(hl.ContainsKey(item.CompanyId))
                    {
                        var tmp = hl[item.CompanyId];
                        sr.high52week = tmp.High;
                        sr.low52week = tmp.Low;
                        var array = hl[item.CompanyId].UpDown30Days.ToCharArray();
                        Array.Reverse(array);
                        sr.upDown = new String(array);
                        sr.upDown = (sr.upDown.Length > 30 ? sr.upDown.Substring(0, 30) : sr.upDown);
                    }
                    sr.change = Math.Round(100 * (item.Close - item.PrevClose)/item.PrevClose, 2);

                    report.Add(sr);
                }
            }
            return report;
        }
    }
}
