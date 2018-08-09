using System;
using System.Collections.Generic;
using System.Linq;
using MarketData;
using MarketData.StockDatabase;

namespace Vue.Controllers
{
    public class CompanyInformation
    {
        public string companyName;
        public string symbol;
        public string IsinNumber;
        public DateTime DateOfListing;
    }

    public class ETFInformation
    {
        public string etfName;
        public string symbol;
        public string Underlying;
        public DateTime DateOfListing;
    }

    public class ListOfCompanyIndex
    {
        public List<CompanyInformation> CompanyList;
        public List<ETFInformation> EtfList;
        public List<string> IndexList;
        public ListOfCompanyIndex(List<EquityInformationTable> company, List<IndexInformationTable> index)
        {
            CompanyList = company.Where(x => x.IsETF == false).Select(x => new CompanyInformation() {
                                companyName = x.CompanyName,
                                symbol = x.Symbol,
                                IsinNumber = x.ISINNumber,
                                DateOfListing = x.DateOfListing
                            }).ToList();
            EtfList = company.Where(x => x.IsETF == true).Select(x => new ETFInformation() {
                                etfName = x.CompanyName,
                                symbol = x.Symbol,
                                Underlying = x.Underlying,
                                DateOfListing = x.DateOfListing
                            }).ToList();

           IndexList = index.Select(x => x.IndexName).ToList();
        }
    }
}
