using MassTransit;
using Microservices.Common.BackgroundServices;
using Microservices.Common.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Notification.WebApi.Consumers;
using Notification.WebApi.Hubs;
using System.Linq;

namespace Notification.WebApi
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
            services.AddSignalR();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notification.WebApi", Version = "v1" });
            });

            //services.Configure<ApiBehaviorOptions>(o => { o.SuppressModelStateInvalidFilter = true; });
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
            });

            services.AddMassTransit(x =>
            {
                x.AddConsumer<NotificationConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ReceiveEndpoint("Notification", e =>
                    {
                        e.ConfigureConsumer<NotificationConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();
            services.AddHostedService<GlobalExceptionBackgroundService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");
            app.UseResponseCompression();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notificationHub");
            });
        }
    }
}
