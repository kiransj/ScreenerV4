using System;

namespace MarketData.NseMarket
{
    class NseURLs
    {
        public string EquityListUrl { get { return "https://www.nseindia.com/content/equities/EQUITY_L.csv"; }}
        public string ETFListUrl { get { return "https://www.nseindia.com/content/equities/eq_etfseclist.csv"; }}
        public string CompanyToIndustryMappingUrl { get { return "https://www.nseindia.com/content/indices/ind_nifty500list.csv"; }}
        public string BhavUrl { get { return $"https://www.nseindia.com/content/historical/EQUITIES/{yyyy}/{MMM}/cm{ddMMMyyyy}bhav.csv.zip"; }}
        public string IndexBhavUrl { get { return $"https://www.nseindia.com/content/indices/ind_close_all_{dd}{MM}{yyyy}.csv"; }}
        public string DeliveryPositionUrL { get { return $"https://www.nseindia.com/archives/equities/mto/MTO_{dd}{MM}{yyyy}.DAT"; }}
        public string PRZipfileUrl { get { return $"https://www.nseindia.com/archives/equities/bhavcopy/pr/PR{dd}{MM}{yy}.zip"; }}
        public string BhavFilename { get { return $"cm{ddMMyy}bhav.csv"; }}
        public string ETFBhavFilename { get { return  $"etf{ddMMyy}.csv"; }}
        public string CircuitBreakerFilename { get { return  $"bh{ddMMyy}.csv"; }}
        public string HighLow52WeekFilename { get { return $"Pd{ddMMyy}.csv"; }}

        private DateTime date;
        private string dd, MM, yy, yyyy, MMM, ddMMyy, ddMMMyyyy;
        public NseURLs(DateTime date)
        {
            this.date = date;
            dd = date.ToString("dd");
            MM = date.ToString("MM");
            yy = date.ToString("yy");
            MMM = date.ToString("MMM").ToUpper();
            ddMMyy = date.ToString("ddMMyy").ToUpper();
            ddMMMyyyy = date.ToString("ddMMMyyyy").ToUpper();
            yyyy = date.ToString("yyyy");
        }

        public override string ToString()
        {
            string output = "\n";

            output += $"\t\tEquity List     : {EquityListUrl}\n";
            output += $"\t\tETF List        : {ETFListUrl}\n";
            output += $"\t\tIndustryMapping : {CompanyToIndustryMappingUrl}\n";
            output += $"\t\tBhav            : {BhavUrl}\n";
            output += $"\t\tIndex Bhav      : {IndexBhavUrl}\n";
            output += $"\t\tDeliveryPosition: {DeliveryPositionUrL}\n";
            output += $"\t\tPR Zipfile      : {PRZipfileUrl}\n";
            output += $"\t\tBhav file       : {BhavFilename}\n";
            output += $"\t\tETFBhav file    : {BhavFilename}\n";
            output += $"\t\tCircuitBrk File : {CircuitBreakerFilename}\n";
            output += $"\t\t52Week HL File  : {HighLow52WeekFilename}\n";

            return output;
        }
    }
    public class MarketAPI
    {

    }
}