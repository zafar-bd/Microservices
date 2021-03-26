using System.Collections.Generic;
using System.Diagnostics;
using FluentValidation.AspNetCore;
using MassTransit;
using MediatR;
using Microservices.Common.BackgroundServices;
using Microservices.Common.Filters;
using Microservices.Sales.Cqrs.Queries;
using Microservices.Sales.Data.Context;
using Microservices.Sales.Dtos;
using Microservices.Sales.Services;
using Microservices.Sales.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Sales.WebApi.Helpers;

namespace Sales.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string conStr = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<SalesDbContext>(options =>
            {
                options.LogTo(tsql => Debug.Write(tsql));
                options.UseSqlServer(conStr);
            });

            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IRequestHandler<SalesQueryDto, IEnumerable<SalesViewModel>>, SalesQueriesHandler>();
            services.AddScoped<IRequestHandler<AvailableStockQueryDto, bool>, SalesQueriesHandler>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sales.WebApi", Version = "v1" });
            });


            services.AddHttpContextAccessor();
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["oidc:Authority"];
                    options.Audience = Configuration["oidc:Audience"];
                });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.WithOrigins("https://localhost:44342")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });

            services.AddAutoMapper(typeof(AutomappperProfile));
            services.AddMediatR(typeof(Startup));

            services.Configure<ApiBehaviorOptions>(o => { o.SuppressModelStateInvalidFilter = true; });
            services
             .AddControllers()
             .AddMvcOptions(options =>
             {
                 var policyBuilder = new AuthorizationPolicyBuilder();
                 policyBuilder.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                 var policy = policyBuilder.RequireAuthenticatedUser().Build();
                 options.Filters.Add(new AuthorizeFilter(policy));
                 options.Filters.Add(typeof(GlobalExceptionFilter));
                 options.Filters.Add(typeof(ValidateModelStateFilter));
             })
                .AddFluentValidation(v => v.RegisterValidatorsFromAssemblyContaining(typeof(Startup)));

            services.AddCaching(Configuration);
            
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq();
            });

            services.AddMassTransitHostedService();
            services.AddHostedService<GlobalExceptionBackgroundService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sales.WebApi v1"));
            }
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
