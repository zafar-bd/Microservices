using System;
using System.Diagnostics;
using MassTransit;
using Microservices.Sales.Data.Context;
using Microservices.Sales.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sales.Processor.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Order Processor";
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var qName = hostContext.Configuration["QName"];
                    services.AddScoped<ISalesService, SalesService>();
                    services.AddScoped<IStockService, StockService>();

                    string conStr = hostContext.Configuration["ConnectionStrings:DefaultConnection"];

                    services.AddDbContext<SalesDbContext>(options =>
                    {
                        options.LogTo(sql => Debug.Write(sql));
                        options.UseSqlServer(conStr);
                    });

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<SalesConsumer>();
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ReceiveEndpoint(qName, e =>
                            {
                                e.ConfigureConsumer<SalesConsumer>(context);
                            });
                        });
                    });

                    services.AddMassTransitHostedService();
                });
    }
}
