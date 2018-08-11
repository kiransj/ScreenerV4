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
        public IActionResult GetStockHistory(string symbol)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(symbol);
            string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            Globals.Log.Info($"History for {returnValue}");
            return Ok(StockReport.GetStockHistory(returnValue));
        }

        [HttpGet("[action]")]
        public ActionResult UpdateDataToLatest()
        {
            StockServices stockService = new StockServices();
            return Ok(stockService.UpdateStockDataToToday());
        }

        [HttpGet("[action]")]
        public ActionResult LogFile()
        {
            return File(System.IO.File.ReadAllBytes(Globals.Options.LogFileName), MediaTypeNames.Text.Plain);
        }
    }
}
