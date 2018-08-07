using System.Text;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using Helper;
using System.Linq;
using System.Collections.Generic;

namespace MarketData.NseMarket
{
    class CSVEquityInformation: CsvMapping<EquityInformation>
    {
        public CSVEquityInformation() : base()
        {
            MapProperty(0, x => x.Symbol);
            MapProperty(1, x => x.CompanyName);
            MapProperty(2, x => x.Series);
            MapProperty(3, x => x.DateOfListing);
            MapProperty(4, x => x.PaidUpValue);
            MapProperty(5, x => x.MarketLot);
            MapProperty(6, x => x.IsinNumber);
            MapProperty(7, x => x.FaceValue);
        }
    }

    class CSVETFInformation: CsvMapping<ETFInformation>
    {
        public CSVETFInformation() : base()
        {
            MapProperty(0, x => x.Symbol);
            MapProperty(1, x => x.Underlying);
            MapProperty(2, x => x.ETFName);
            MapProperty(3, x => x.DateOfListing);
            MapProperty(4, x => x.MarketLot);
            MapProperty(5, x => x.IsinNumber);
            MapProperty(6, x => x.FaceValue);
        }
    }

    class CSVCompanyToIndustryMapping: CsvMapping<CompanyToIndustryMapping>
    {
        public CSVCompanyToIndustryMapping() : base()
        {
            MapProperty(0, x => x.CompanyName);
            MapProperty(1, x => x.Industry);
            MapProperty(2, x => x.Symbol);
            MapProperty(3, x => x.Series);
            MapProperty(4, x => x.ISINCode);
        }
    }

    class CsvBhav : CsvMapping<Bhav>
    {
        public CsvBhav() : base()
        {
            MapProperty(0, x => x.Symbol);
            MapProperty(1, x => x.Series);
            MapProperty(2, x => x.Open);
            MapProperty(3, x => x.High);
            MapProperty(4, x => x.Low);
            MapProperty(5, x => x.Close);
            MapProperty(6, x => x.Last);
            MapProperty(7, x => x.PrevClose);
            MapProperty(8, x => x.TotTradedQty);
            MapProperty(9, x => x.TotTradedValue);
            MapProperty(10, x => x.TimeStamp);
            MapProperty(11, x => x.TotalTrades);
            MapProperty(12, x => x.ISINNumber);
        }
    }

    class CsvETFBhav : CsvMapping<ETFBhav>
    {
        public CsvETFBhav() : base()
        {
            MapProperty(0, x => x.Market);
            MapProperty(1, x => x.Series);
            MapProperty(2, x => x.Symbol);
            MapProperty(3, x => x.Security);
            MapProperty(4, x => x.PrevClosePrice);
            MapProperty(5, x => x.OpenPrice);
            MapProperty(6, x => x.HighPrice);
            MapProperty(7, x => x.LowPrice);
            MapProperty(8, x => x.ClosePrice);
            MapProperty(9, x => x.NetTradedValue);
            MapProperty(10, x => x.NetTradedQty);
            MapProperty(11, x => x.TotalTrades);
            MapProperty(12, x => x.High52Week);
            MapProperty(13, x => x.Low52Week);
            MapProperty(14, x => x.Underlying);
        }
    }

    class CsvIndexBhav : CsvMapping<IndexBhav>
    {
        public CsvIndexBhav() : base()
        {
            MapProperty(0, x => x.IndexName);
            MapProperty(1, x => x.IndexDate);
            MapProperty(2, x => x.OpenValue);
            MapProperty(3, x => x.HighValue);
            MapProperty(4, x => x.LowValue);
            MapProperty(5, x => x.CloseValue);
            MapProperty(6, x => x.PointsChange);
            MapProperty(7, x => x.PointsChangePct);
            MapProperty(8, x => x.Volume);
            MapProperty(9, x => x.TurnOver);
            MapProperty(10, x => x.PE);
            MapProperty(11, x => x.PB);
            MapProperty(12, x => x.DivYield);
        }
    }

    class CsvDeliveryPositionMapping : CsvMapping<DeliveryPosition>
    {
        public CsvDeliveryPositionMapping() : base()
        {
            MapProperty(0, x => x.RecordType);
            MapProperty(1, x => x.SerialNumber);
            MapProperty(2, x => x.SecurityName);
            MapProperty(3, x => x.Series);
            MapProperty(4, x => x.QtyTraded);
            MapProperty(5, x => x.DeliverableQty);
            MapProperty(6, x => x.DeliveryPercentage);
        }
    }

    class CsvCircuitBreakerMapping : CsvMapping<CircuitBreaker>
    {
        public CsvCircuitBreakerMapping() : base()
        {
            MapProperty(0, x => x.Symbol);
            MapProperty(1, x => x.Series);
            MapProperty(2, x => x.CompanyName);
            MapProperty(3, x => x.HighLow);
        }
    }

    class CsvHighLow52WeekMapping: CsvMapping<HighLow52week>
    {
        public CsvHighLow52WeekMapping() : base()
        {
            MapProperty(0, x => x.Market);
            MapProperty(1, x => x.Series);
            MapProperty(2, x => x.Symbol);
            MapProperty(3, x => x.Security);
            MapProperty(14, x => x.High52week);
            MapProperty(15, x => x.Low52week);
        }
    }

    public class CsvParser
    {
        public List<EquityInformation> ParseEquityInformationFile(string filename)
        {
            CsvParser<EquityInformation> csvParser = new CsvParser<EquityInformation>(new CsvParserOptions(true, ','), new CSVEquityInformation());
            Globals.Log.Debug($"Parsing NSE Company Information CSV file {filename}");

            return csvParser.ReadFromFile(filename, Encoding.ASCII)
                            .Select(x => x.Result)
                            .Where(x => x != null)
                            .ToList();
        }

        public List<ETFInformation> ParseETFInformationFile(string filename)
        {
            CsvParser<ETFInformation> csvParser = new CsvParser<ETFInformation>(new CsvParserOptions(true, ','), new CSVETFInformation());
            Globals.Log.Debug($"Parsing NSE ETF Information CSV file {filename}");

            return csvParser.ReadFromFile(filename, Encoding.ASCII)
                            .Select(x => x.Result)
                            .Where(x => x != null)
                            .ToList();
        }


        public List<IndexBhav> ParseIndexBhavFile(string filename)
        {
            CsvParser<IndexBhav> csvParser = new CsvParser<IndexBhav>(new CsvParserOptions(true, ','), new CsvIndexBhav());
            Globals.Log.Debug($"Parsing Index Bhav Information CSV file {filename}");

            return csvParser.ReadFromFile(filename, Encoding.ASCII)
                            .Select(x => x.Result)
                            .Where(x => x != null)
                            .ToList();
        }

        public List<Bhav> ParseBhavFile(string filename)
        {
            CsvParser<Bhav> csvParser = new CsvParser<Bhav>(new CsvParserOptions(true, ','), new CsvBhav());
            Globals.Log.Debug($"Parsing Bhav Information CSV file {filename}");

            return csvParser.ReadFromFile(filename, Encoding.ASCII)
                            .Select(x => x.Result)
                            .Where(x => x != null)
                            .ToList();
        }

        public List<ETFBhav> ParseETFBhavFile(string filename)
        {
            CsvParser<ETFBhav> csvParser = new CsvParser<ETFBhav>(new CsvParserOptions(true, ','), new CsvETFBhav());
            Globals.Log.Debug($"Parsing ETF Bhav Information CSV file {filename}");

            return csvParser.ReadFromFile(filename, Encoding.ASCII)
                            .Select(x => x.Result)
                            .Where(x => x != null)
                            .ToList();
        }

        public List<DeliveryPosition> ParseDeliveryPositionFile(string filename)
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            CsvDeliveryPositionMapping csvMapper = new CsvDeliveryPositionMapping();
            CsvParser<DeliveryPosition> csvParser = new CsvParser<DeliveryPosition>(csvParserOptions, csvMapper);

            Globals.Log.Debug($"Parsing NSE Delivery Position CSV file {filename}");
            return csvParser.ReadFromFile(filename, Encoding.ASCII)
                            .Select(x => x.Result)
                            .Where(x => x != null)
                            .ToList();
        }


        public List<CompanyToIndustryMapping> ParseCompanyToIndustryMappingFile(string filename)
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            CSVCompanyToIndustryMapping csvMapper = new CSVCompanyToIndustryMapping();
            CsvParser<CompanyToIndustryMapping> csvParser = new CsvParser<CompanyToIndustryMapping>(csvParserOptions, csvMapper);

            Globals.Log.Debug($"Parsing NSE Company to industry CSV file {filename}");
            return csvParser.ReadFromFile(filename, Encoding.ASCII)
                            .Select(x => x.Result)
                            .Where(x => x != null)
                            .ToList();
        }

        public List<CircuitBreaker> ParseCircuitBreakerFile(string filename)
        {
            CsvParser<CircuitBreaker> csvParser = new CsvParser<CircuitBreaker>(new CsvParserOptions(true, ','), new CsvCircuitBreakerMapping());

            Globals.Log.Debug($"Parsing NSE Circuit Breaker file CSV file {filename}");
            return csvParser.ReadFromFile(filename, Encoding.ASCII)
                            .Select(x => x.Result)
                            .Where(x => x != null)
                            .ToList();
        }

        public List<HighLow52week> ParseHighLow52WeekFile(string filename)
        {
            CsvParser<HighLow52week> csvParser = new CsvParser<HighLow52week>(new CsvParserOptions(true, ','), new CsvHighLow52WeekMapping());

            Globals.Log.Debug($"Parsing HighLow52week file CSV file {filename}");
            return csvParser.ReadFromFile(filename, Encoding.ASCII)
                            .Select(x => x.Result)
                            .Where(x => (x != null) && (x.Symbol.Length >= 2))
                            .ToList();
        }
    }
}