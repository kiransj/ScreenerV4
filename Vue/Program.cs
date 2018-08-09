using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Helper;
namespace vue
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Globals.InitGlobals("../MarketData/Options.json");
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            Globals.Log.Info($"Starting Server on port {Globals.Options.httpServer.Port}");
            return WebHost.CreateDefaultBuilder(args)
                    .UseUrls($"http://*:{Globals.Options.httpServer.Port}")
                    .UseStartup<Startup>()
                    .Build();
        }
    }
}
