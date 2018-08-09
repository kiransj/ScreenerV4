using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
