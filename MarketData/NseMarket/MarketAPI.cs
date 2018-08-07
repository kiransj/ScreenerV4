using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;


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
        public string BhavFilename { get { return $"cm{ddMMMyyyy}bhav.csv"; }}
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
        public List<Bhav> BhavData;
        public List<ETFBhav> ETFBhavData;
        public List<CompanyToIndustryMapping> CompanyToIndustry;
        public List<DeliveryPosition> deliveryPosition;
        public List<CircuitBreaker> circuitBreaker;
        public List<HighLow52week> highLow52Week;

        public override string ToString()
        {
            return "\n" +
                   $"Companies: {Equitys.Count}\n" +
                   $"ETF: {Etfs.Count}\n" +
                   $"Index: {Indexes.Count}\n" +
                   $"IndexBhav: {IndexDailyData.Count}\n"  +
                   $"Bhav: {BhavData.Count}\n" +
                   $"ETFBhav: {ETFBhavData.Count}\n" +
                   $"Industry: {CompanyToIndustry.Count}\n" +
                   $"Delivery Position: {deliveryPosition.Count}\n" +
                   $"Circuit Breakers: {circuitBreaker.Count}\n" +
                   $"52 Week H/L: {highLow52Week.Count}\n" ;
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

        public async Task<NseDailyData> GetDailyData(DateTime date)
        {
            NseURLs nseUrls = new NseURLs(date);
            NseDailyData list = new NseDailyData();
            Dictionary<string, string> urlToFileMapping = new Dictionary<string, string>();
            string folder = $"{Options.app.TmpFolder}/{date.ToString("ddMMyyyy")}";

            // Delete the folder if it exists then create the folder
            if(Directory.Exists(folder)) {
                Globals.Log.Info($"Deleting folder {folder}");
                Directory.Delete(folder, true);
            }
            Globals.Log.Info($"Creating folder {folder}");
            Directory.CreateDirectory(folder);

            urlToFileMapping[nseUrls.BhavUrl] =  $"{folder}/bhav.csv.zip";
            urlToFileMapping[nseUrls.PRZipfileUrl] =  $"{folder}/PR.zip";
            urlToFileMapping[nseUrls.EquityListUrl] =  $"{folder}/equity.csv";
            urlToFileMapping[nseUrls.ETFListUrl] =  $"{folder}/etf.csv";
            urlToFileMapping[nseUrls.IndexBhavUrl] =  $"{folder}/indexBhav.csv";
            urlToFileMapping[nseUrls.CompanyToIndustryMappingUrl] =  $"{folder}/mapping.csv";
            urlToFileMapping[nseUrls.DeliveryPositionUrL] =  $"{folder}/MTO.csv";

            List<Task> task = new List<Task>();
            foreach(var item in urlToFileMapping)
            {
                task.Add(fileDownloader.Download(item.Key, item.Value));
            }
            await Task.WhenAll(task.ToArray());

            Globals.Log.Info($"Extracting Zip files to {folder}");
            ZipFile.ExtractToDirectory(urlToFileMapping[nseUrls.BhavUrl], folder);
            ZipFile.ExtractToDirectory(urlToFileMapping[nseUrls.PRZipfileUrl], folder);

            NseDailyData dailyData = new NseDailyData();

            // Parse all the downloaded Data
            dailyData.Equitys = csvParser.ParseEquityInformationFile(urlToFileMapping[nseUrls.EquityListUrl]);
            dailyData.Etfs = csvParser.ParseETFInformationFile(urlToFileMapping[nseUrls.ETFListUrl]);
            dailyData.IndexDailyData = csvParser.ParseIndexBhavFile(urlToFileMapping[nseUrls.IndexBhavUrl]);
            dailyData.deliveryPosition = csvParser.ParseDeliveryPositionFile(urlToFileMapping[nseUrls.DeliveryPositionUrL]);
            dailyData.CompanyToIndustry = csvParser.ParseCompanyToIndustryMappingFile(urlToFileMapping[nseUrls.CompanyToIndustryMappingUrl]);

            //Parse the unzipped files
            dailyData.BhavData = csvParser.ParseBhavFile($"{folder}/{nseUrls.BhavFilename}");
            dailyData.ETFBhavData = csvParser.ParseETFBhavFile($"{folder}/{nseUrls.ETFBhavFilename}");
            dailyData.circuitBreaker = csvParser.ParseCircuitBreakerFile($"{folder}/{nseUrls.CircuitBreakerFilename}");
            dailyData.highLow52Week = csvParser.ParseHighLow52WeekFile($"{folder}/{nseUrls.HighLow52WeekFilename}");

            // Computed Valus
            dailyData.Indexes = dailyData.IndexDailyData.Select(x => new IndexInformation(x.IndexName))
                                                        .OrderBy(x => x.IndexName)
                                                        .Distinct()
                                                        .ToList();
            return dailyData;
        }
        public async Task<NseDailyData> GetSecurityList()
        {
            NseURLs nseUrls = new NseURLs(DateTime.Now.AddDays(-1));
            NseDailyData list = new NseDailyData();
            string folder = Options.app.TmpFolder;
            string date = DateTime.Now.ToString("ddMMyyyy");


            string equityFile = $"{folder}/equity.csv";
            string etfFile = $"{folder}/etf.csv";
            string indexBhavFile = $"{folder}/indexBhav_{date}.csv";
            string bhavZipFile = $"{folder}/bhav_{date}.csv.zip";

            var t1 = fileDownloader.Download(nseUrls.EquityListUrl, equityFile);
            var t2 = fileDownloader.Download(nseUrls.ETFListUrl, etfFile);
            var t3 = fileDownloader.Download(nseUrls.IndexBhavUrl, indexBhavFile);
            var t4 = fileDownloader.Download(nseUrls.BhavUrl, bhavZipFile);
            await Task.WhenAll(t1, t2, t3, t4);
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