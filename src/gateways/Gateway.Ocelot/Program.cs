using Microservices.Common.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Gateway.Ocelot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GlobalLogger.ConfigureLog();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                       .ConfigureAppConfiguration(ic => ic.AddJsonFile("ocelot.json"))
                       .UseStartup<Startup>();
                });
    }
}
