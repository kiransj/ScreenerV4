﻿using System;
using System.Diagnostics;
using Helper;
using MarketData.NseMarket;
namespace MarketData
{
    class Program
    {
        static Logger log = Logger.GetLoggerInstance();
        static void Main(string[] args)
        {
            Globals.InitGlobals("Options.json");
            NseURLs url = new NseURLs(DateTime.Now);
            FileDownloader fileDownloader = new FileDownloader();
            string data;
            fileDownloader.DownloadAsString(url.EquityListUrl, out data);
            Globals.log.Info(data);
        }
    }
}
