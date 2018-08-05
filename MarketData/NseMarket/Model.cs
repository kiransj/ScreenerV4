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
    }

    // Information in https://www.nseindia.com/archives/equities/mto/MTO_DDMMYYYY.DAT
    // Ignore first 4 lines
    public class DeliveryPosition
    {
        public int RecordType { get; set; }
        public int SerialNumber { get; set; }
        public string SecurityName { get; set; }
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
        public float PrevClosePrice {get; set; }
        public float OpenPrice {get; set; }
        public float HighPrice {get; set; }
        public float LowPrice {get; set; }
        public float ClosePrice {get; set; }
        public float NetTradedValue {get; set; }
        public float NetTradedQty {get; set; }
        public float TotalTrades {get; set; }
        public float High52Week {get; set; }
        public float Low52Week {get; set; }
        public string Underlying {get; set; }
    }

    //https://www.nseindia.com/content/indices/ind_close_all_02082018.csv
    public class IndexBhav
    {
        public string IndexName { get; set; }
        public DateTime IndexDate {get; set; }
        public float OpenValue {get; set; }
        public float HighValue {get; set; }
        public float LowValue {get; set; }
        public float CloseValue {get; set; }
        public float PointsChange {get; set; }
        public float PointsChangePct {get; set; }
        public float Volume {get; set; }
        public float TurnOver {get; set; }
        public float PE {get; set; }
        public float PB {get; set; }
        public float DivYield {get; set; }
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
}