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
            if(args.Count() == 1)
            {
                Console.WriteLine($"Options file {args[0]} passed from cmd line");
                Globals.InitGlobals(args[0]);
            }
            else if(args.Count() == 0)
            {
                Console.WriteLine("Using Default Options file");
                Globals.InitGlobals("../MarketData/Options.json");
            }
            else
            {
                Console.WriteLine("Wrong argument. Usage ./a.out <optionsFile>");
                Environment.Exit(2);
            }
            BuildWebHost().Run();
        }

        public static IWebHost BuildWebHost()
        {
            Globals.Log.Info($"Starting Server on port {Globals.Options.httpServer.Port}");
            return WebHost.CreateDefaultBuilder()
                    .UseUrls($"http://*:{Globals.Options.httpServer.Port}")
                    .UseStartup<Startup>()
                    .Build();
        }
    }
}
