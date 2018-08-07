using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helper;

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

    public class NseDailyData
    {
        public List<EquityInformation> Equitys;
        public List<ETFInformation> Etfs;
        public List<IndexInformation> Indexes;
        public List<IndexBhav> IndexDailyData;

        public override string ToString()
        {
            return $"Companies: {Equitys.Count}, ETF: {Etfs.Count}, Index: {Indexes.Count}, IndexBhav: {IndexDailyData.Count}";
        }
    }

    public class MarketAPI
    {
        private CsvParser csvParser;
        private FileDownloader fileDownloader;
        public MarketAPI()
        {
            csvParser = new CsvParser();
            fileDownloader = new FileDownloader();
        }
        public async Task<NseDailyData> GetSecurityList()
        {
            NseURLs nseUrls = new NseURLs(DateTime.Now.AddDays(-1));
            string folder = Options.app.TmpFolder;
            NseDailyData list = new NseDailyData();

            string date = DateTime.Now.ToString("ddMMyyyy");

            string equityFile = $"{folder}/equity.csv";
            string etfFile = $"{folder}/etf.csv";
            string indexBhavFile = $"{folder}/indexBhav_{date}.csv";
            string bhavZipFile = $"{folder}/bhav_{date}.csv.zip";

            var t1 = fileDownloader.Download(nseUrls.EquityListUrl, equityFile);
            var t2 = fileDownloader.Download(nseUrls.ETFListUrl, etfFile);
            var t3 = fileDownloader.Download(nseUrls.IndexBhavUrl, indexBhavFile);
            var t4 = fileDownloader.Download(nseUrls.BhavUrl, bhavZipFile);
            await Task.WhenAll(t1, t2, t3);
            //Task.WaitAll(t1, t2, t3);

            list.Equitys = csvParser.ParseEquityInformationFile(equityFile);
            list.Etfs = csvParser.ParseETFInformationFile(etfFile);
            list.IndexDailyData = csvParser.ParseIndexBhavFile(indexBhavFile);

            list.Indexes = list.IndexDailyData.Select(x => new IndexInformation(x.IndexName))
                                              .OrderBy(x => x.IndexName)
                                              .Distinct()
                                              .ToList();
            Globals.Log.Info($"GetSecurityList() returning {list.ToString()}");
            return list;
        }
    }
}