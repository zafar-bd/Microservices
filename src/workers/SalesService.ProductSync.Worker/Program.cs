using System;
using System.Diagnostics;
using MassTransit;
using Microservices.Common.Helpers;
using Microservices.Sales.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SalesService.ProductSync.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Product Consumer For Sales Microservice";
            GlobalLogger.ConfigureLog();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    var qName = hostContext.Configuration["QName"];

                    string conStr = hostContext.Configuration["ConnectionStrings:DefaultConnection"];

                    services.AddDbContext<SalesDbContext>(options =>
                    {
                        options.LogTo(tsql => Debug.Write(tsql));
                        options.UseSqlServer(conStr);
                    });

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<ProductConsumer>();
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ReceiveEndpoint(qName, e =>
                            {
                                e.ConfigureConsumer<ProductConsumer>(context);
                            });
                        });
                    });

                    services.AddMassTransitHostedService();
                });
    }
}
