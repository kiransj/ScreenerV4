using System;
using System.Collections.Generic;
using System.Linq;
using Helper;
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

    public class NiftyOptionsReport
    {
        public int optionsId;
        public DateTime expiryDate;
        public double strikePrice;
        public bool callOptions;
        public double open;
        public double close;
        public double high;
        public double low;
        public long openIntrest;
        public long tradedQty;
        public long numOfCont;
        public long numOfTrade;
        public double notionalValue;
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
        public double change5d;
        public double change30d;
        public double change60d;
        public double change120d;
        public double hlp;
        public double DelQtyChange;
        public double marketCap;

        public StockDailyReport()
        {
            high52week = -1;
            low52week = -1;
            upDown = "";
            circuitBreaker = "";
            change5d = -1;
            change30d = -1;
            hlp = -1;
            DelQtyChange = -1;
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

        static public List<NiftyOptionsReport> GetNiftyOptionsData(DateTime date)
        {
            StockServices ss = new StockServices();
            return ss.GetNiftyOptionsData(date).Select(x => new NiftyOptionsReport() {
                optionsId = x.OptionId,
                expiryDate = ss.DayToDate(x.ExpDay),
                strikePrice = x.StrikePrice,
                callOptions = x.CallOption,
                open = x.Open,
                close = x.Close,
                high = x.High,
                low = x.Low,
                openIntrest = (long)x.OpenIntrest,
                tradedQty  = (long)x.TradedQty,
                numOfCont = (long)x.NumOfCont,
                numOfTrade = (long)x.NumOfTrade,
                notionalValue = x.NotionalValue
            }).ToList();
        }

        static public List<StockDailyReport> GetStockReport(DateTime date)
        {
            StockServices stockService = new StockServices();

            (bhav, ohlc, highLow) = stockService.GetStockReport(date);
            mapping = stockService.GetCompanyIdToSymbol();

            Dictionary<int, string> circuitBreaker = ohlc.Select(x => new { x.CompanyId, x.HighLow}).ToDictionary(x => x.CompanyId, x => x.HighLow);
            Dictionary<int, HighLow52WeekTable> hl = highLow.ToDictionary(x => x.CompanyId, x => x);
            DateTime[] dates = stockService.GetTradedDates().ToArray();

            int index = 0;
            for(int i = 0; i < dates.Length; i++)
            {
                if(dates[i].Date.Year == date.Date.Year && dates[i].Date.Day == date.Date.Day && dates[i].Date.Month== date.Date.Month)
                {
                    index = i;
                    break;
                }
            }

            var bhav2d = stockService.GetStockReport(dates[index+1]).bhav.ToDictionary(x => x.CompanyId, x => x.TotalDeliveredQty);
            var bhav5d = stockService.GetStockReport(dates[index+5]).bhav.ToDictionary(x => x.CompanyId, x => x.Close);
            var bhav30d = stockService.GetStockReport(dates[index+30]).bhav.ToDictionary(x => x.CompanyId, x => x.Close);
            var bhav60d = stockService.GetStockReport(dates[index+90]).bhav.ToDictionary(x => x.CompanyId, x => x.Close);
            var bhav120d = stockService.GetStockReport(dates[index+120]).bhav.ToDictionary(x => x.CompanyId, x => x.Close);

            var marketCap = stockService.GetMarketCap();
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
                    sr.change = Math.Round(100 * (item.Close - item.PrevClose)/item.PrevClose, 2);
                    if(hl.ContainsKey(item.CompanyId))
                    {
                        var tmp = hl[item.CompanyId];
                        sr.high52week = tmp.High;
                        sr.low52week = tmp.Low;
                        sr.hlp = Math.Round(100.0 * (sr.close - tmp.Low)/(tmp.High - tmp.Low), 2);
                        var array = hl[item.CompanyId].UpDown30Days.ToCharArray();
                        Array.Reverse(array);
                        sr.upDown = new String(array);
                        sr.upDown = (sr.upDown.Length > 30 ? sr.upDown.Substring(0, 30) : sr.upDown);
                    }

                    if(bhav5d.ContainsKey(item.CompanyId))
                    {
                        var price = bhav5d[item.CompanyId];
                        sr.change5d = Math.Round(100 * (item.Close - price)/price, 2);
                    }
                    if(bhav30d.ContainsKey(item.CompanyId))
                    {
                        var price = bhav30d[item.CompanyId];
                        sr.change30d = Math.Round(100 * (item.Close - price)/price, 2);
                    }
                    if(bhav60d.ContainsKey(item.CompanyId))
                    {
                        var price = bhav60d[item.CompanyId];
                        sr.change60d = Math.Round(100 * (item.Close - price)/price, 2);
                    }
                    if(bhav120d.ContainsKey(item.CompanyId))
                    {
                        var price = bhav120d[item.CompanyId];
                        sr.change120d = Math.Round(100 * (item.Close - price)/price, 2);
                    }

                    if(bhav2d.ContainsKey(item.CompanyId))
                    {
                        sr.DelQtyChange = Math.Round(1.0 * item.TotalDeliveredQty/bhav2d[item.CompanyId], 2);
                    }

                    if(marketCap.ContainsKey(item.CompanyId))
                    {
                        sr.marketCap = Math.Round(marketCap[item.CompanyId] * sr.close/10000000.0, 2);
                    }

                    report.Add(sr);
                }
            }
            return report;
        }
    }
}
