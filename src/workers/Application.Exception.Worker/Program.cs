using MassTransit;
using Microservices.Exceptions.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using Microservices.Common.Helpers;
using Serilog;

namespace Application.Exception.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Microservice Global Exceptions";
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

                    services.AddDbContext<ExceptionDbContext>(options =>
                    {
                        options.LogTo(tsql => Debug.Write(tsql));
                        options.UseSqlServer(conStr);
                    });

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<ExceptionConsumer>();
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ReceiveEndpoint(qName, e =>
                            {
                                e.ConfigureConsumer<ExceptionConsumer>(context);
                            });
                        });
                    });

                    services.AddMassTransitHostedService();
                });
    }
}
