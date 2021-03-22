using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Ui.Blazor.WA.HttpClients;
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

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("oidc", options.ProviderOptions);
            });

            builder.Services.AddHttpClient<ProductHttpClient>("api_gateway", client =>
            {
                client.BaseAddress = new Uri("https://localhost:44315");
            })
           .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("api_gateway"));

            await builder.Build().RunAsync();
        }
    }
}
