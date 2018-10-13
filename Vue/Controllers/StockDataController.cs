using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Helper;
using MarketData;
using Microsoft.AspNetCore.Mvc;
using Vue.Controllers;

namespace vue.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StockDataController : Controller
    {
        private StockServices stockService;
        public StockDataController()
        {
            stockService = new StockServices();
        }

        [HttpGet("[action]")]
        public IActionResult GetCompanyList()
        {
            var result = stockService.GetListOfEquityIndex();
            ListOfCompanyIndex list = new ListOfCompanyIndex(result.Companies, result.Indexes);
            return Ok(list);
        }

        [HttpGet("[action]")]
        public IActionResult GetStockReport(DateTime date)
        {
            var result = StockReport.GetStockReport(date);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult GetLastestStockReport()
        {
            var result = StockReport.GetStockReport(stockService.GetLastUpdatedDate());
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult GetLatestNiftyOptionsData()
        {
            var tradedDays = stockService.GetTradedDates();
            var result = StockReport.GetNiftyOptionsData(tradedDays[0], tradedDays[1]);
            Globals.Log.Info($"Nifty Options report Query");
            return Ok(result);
        }

        [HttpGet("[action]")]
        public ActionResult GetNiftyOptionsDataFor(string expiryDate, long strikePrice, bool callOption)
        {
            var d = DateTime.ParseExact(expiryDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            Globals.Log.Info($"History for Options expiryDate {expiryDate}, stikePrice: {strikePrice}, optionTypeCall: {callOption}");
            return Ok(StockReport.GetNiftyOptionsDataFor(d, strikePrice, callOption));
        }

        [HttpGet("[action]")]
        public IActionResult GetNiftyIndexHistory(string index)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(index);
            string indexName = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            Globals.Log.Info($"History for Index {indexName}");
            return Ok(StockReport.GetNiftyIndexHistory(indexName));
        }

        [HttpGet("[action]")]
        public IActionResult GetStockHistory(string symbol)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(symbol);
            string symbolName = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            Globals.Log.Info($"History for {symbolName}");
            return Ok(StockReport.GetStockHistory(symbolName));
        }

        [HttpGet("[action]")]
        public ActionResult UpdateDataToLatest()
        {
            StockServices stockService = new StockServices();
            return Ok(stockService.UpdateStockDataToToday());
            //return Ok(stockService.UpdateStockDataFor(new DateTime(2018, 9, 11)));
        }

        [HttpGet("[action]")]
        public ActionResult LogFile()
        {
            return File(System.IO.File.ReadAllBytes(Globals.Options.LogFileName), MediaTypeNames.Text.Plain);
        }

        [HttpGet("[action]")]
        public ActionResult AddToFavList(string Symbol, string FavList)
        {
            string symbol = System.Text.ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String(Symbol));
            string favList = System.Text.ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String(FavList));

            return Ok(stockService.AddStockToFavList(symbol, favList));
        }

        [HttpGet("[action]")]
        public ActionResult GetFavLists()
        {
            return Ok(stockService.GetStockFavList().GroupBy(x => x.ListName).Select(x => x.First().ListName).ToList());
        }

        [HttpGet("[action]")]
        public ActionResult GetFavListWithSymbols()
        {
            return Ok(stockService.GetStockFavList());
        }

        [HttpGet("[action]")]
        public ActionResult RemoveToFavList(string Symbol, string FavList)
        {
            string symbol = System.Text.ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String(Symbol));
            string favList = System.Text.ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String(FavList));

            return Ok(stockService.RemoveStockToFavList(symbol, favList));
        }
    }
}
