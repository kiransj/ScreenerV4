using TinyCsvParser.Mapping;

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
            MapProperty(5, x => x.MarketLot);
            MapProperty(6, x => x.IsinNumber);
            MapProperty(7, x => x.FaceValue);
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


}