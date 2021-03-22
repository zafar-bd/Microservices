using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Ui.Blazor.WA.HttpClients;
using Ui.Blazor.WA.Models.App;
using Ui.Blazor.WA.Services;

namespace Ui.Blazor.WA
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

            Endpoints endPoint = null;

            builder.Services.AddOidcAuthentication(options =>
            {
                endPoint = builder.Configuration.GetSection("EndPoints").Get<Endpoints>();
                builder.Configuration.Bind("oidc", options.ProviderOptions);
            });

            builder.Services.AddHttpClient<ProductHttpClient>("api_gateway", client =>
            {
                client.BaseAddress = new Uri(endPoint?.Gateway);
            })
           .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("api_gateway"));

            await builder.Build().RunAsync();
        }
    }
}
