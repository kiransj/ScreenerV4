using System;

namespace MarketData.NseMarket
{
    //https://www.nseindia.com/content/equities/EQUITY_L.csv
    public class EquityInformation
    {
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public string Series { get; set; }
        public DateTime DateOfListing { get; set; }
        public double PaidUpValue { get; set; }
        public int MarketLot { get; set; }
        public string IsinNumber { get; set; }
        public double FaceValue { get; set; }
    }

    //https://www.nseindia.com/content/equities/eq_etfseclist.csv
    public class ETFInformation
    {
        public string Symbol { get; set; }
        public string Underlying { get; set; }
        public string ETFName { get; set; }
        public DateTime DateOfListing { get; set; }
        public int MarketLot { get; set; }
        public string IsinNumber { get; set; }
        public double FaceValue { get; set; }
    }

    // Computed value from IndexBhav
    public class IndexInformation
    {
        public string IndexName { get; set; }
        public IndexInformation(string index)
        {
            IndexName = index;
        }
    }

    //https://www.nseindia.com/content/indices/ind_nifty500list.csv
    public class CompanyToIndustryMapping
    {
        public string CompanyName {get; set; }
        public string Industry {get; set; }
        public string Symbol {get; set; }
        public string Series {get; set; }
        public string ISINCode {get; set; }
    }

    //https://www.nseindia.com/content/historical/EQUITIES/2018/AUG/cm03AUG2018bhav.csv.zip
    public class Bhav
    {
        public string Symbol { get; set; }
        public string Series { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Last { get; set; }
        public double PrevClose { get; set; }
        public long TotTradedQty { get; set; }
        public double TotTradedValue { get; set; }
        public DateTime TimeStamp { get; set; }
        public long TotalTrades { get; set; }
        public string ISINNumber { get; set; }

        public double ChangePct {
            get {
                return Math.Round(100 * (Close - PrevClose)/PrevClose, 2);
            }
        }
    }

    //https://www.nseindia.com/content/indices/ind_close_all_02082018.csv
    public class IndexBhav
    {
        public string IndexName { get; set; }
        public DateTime IndexDate {get; set; }
        public double OpenValue {get; set; }
        public double HighValue {get; set; }
        public double LowValue {get; set; }
        public double CloseValue {get; set; }
        public double PointsChange {get; set; }
        public double PointsChangePct {get; set; }
        public double Volume {get; set; }
        public double TurnOver {get; set; }
        public double PE {get; set; }
        public double PB {get; set; }
        public double DivYield {get; set; }
    }

    // Information in https://www.nseindia.com/archives/equities/mto/MTO_DDMMYYYY.DAT
    // Ignore first 4 lines
    public class DeliveryPosition
    {
        public int RecordType { get; set; }
        public int SerialNumber { get; set; }
        public string Symbol { get; set; }
        public string Series { get; set; }
        public long QtyTraded { get; set; }
        public long DeliverableQty { get; set; }
        public double DeliveryPercentage { get; set; }
    }

    // Information in PRDDMMYY.zip => etfDDMMYY.csv
    public class ETFBhav
    {
        public string Market {get; set; }
        public string Series {get; set; }
        public string Symbol {get; set; }
        public string Security {get; set; } //Company Name
        public double PrevClosePrice {get; set; }
        public double OpenPrice {get; set; }
        public double HighPrice {get; set; }
        public double LowPrice {get; set; }
        public double ClosePrice {get; set; }
        public double NetTradedValue {get; set; }
        public double NetTradedQty {get; set; }
        public double TotalTrades {get; set; }
        public double High52Week {get; set; }
        public double Low52Week {get; set; }
        public string Underlying {get; set; }
    }

    //Information in PRDDMMYY.zip => bhDDMMYY.csv
    public class CircuitBreaker
    {
        public string Symbol { get; set; }
        public string Series { get; set; }
        public string CompanyName { get; set; }
        public string HighLow { get; set; }
    }

    //Information in PRDDMMYY.zip => PdDDMMYY.csv
    public class HighLow52week
    {
        public string Market { get; set; }
        public string Series { get; set; }
        public string Symbol { get; set; }
        public string Security { get; set; }
        public double High52week { get; set; }
        public double Low52week { get; set; }
    }

    public class NiftyOptionsBhav
    {
        public string Instrument { get; set; }
        public string Symbol {get; set;}
        public DateTime ExpDate {get; set;}
        public double StrikePrice {get; set;}
        public string OptType {get; set;}
        public double OpenPrice {get; set;}
        public double HiPrice {get; set;}
        public double LowPrice {get; set;}
        public double ClosePrice {get; set;}
        public double OpenIntrest {get; set;}
        public long TradedQty {get; set;}
        public long NumOfCont {get; set;}
        public long NumOfTrade {get; set;}
        public double NotionalValue {get; set;}
        public double PrVal {get; set;}
    }

    public class MarketCap
    {
        public string Symbol { get; set; }
        public string IsinNumber { get; set; }
        public Int64 numOfShares { get; set; }
    }
}