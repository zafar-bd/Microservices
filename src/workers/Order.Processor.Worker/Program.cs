using MassTransit;
using MediatR;
using Microservices.Order.Cqrs.Queries;
using Microservices.Order.Data.Context;
using Microservices.Order.Dtos;
using Microservices.Order.Services;
using Microservices.Order.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Order.Processor.Worker
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
                    services.AddScoped<IOrderService, OrderService>();
                    services.AddScoped<IStockService, StockService>();

                    string conStr = hostContext.Configuration["ConnectionStrings:DefaultConnection"];

                    services.AddDbContext<OrderDbContext>(options =>
                    {
                        options.LogTo(tsql => Debug.Write(tsql));
                        options.UseSqlServer(conStr);
                    });

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<OrderConsumer>();
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ReceiveEndpoint(qName, e =>
                            {
                                e.ConfigureConsumer<OrderConsumer>(context);
                            });
                        });
                    });

                    services.AddMassTransitHostedService();
                });
    }
}
