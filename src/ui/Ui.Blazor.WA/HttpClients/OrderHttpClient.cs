using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Ui.Blazor.WA.Models.ViewModels;
using System.Net.Http.Json;
using Ui.Blazor.WA.Models.Dto;
using System.Threading;

namespace Ui.Blazor.WA.HttpClients
{
    public class OrderHttpClient
    {
        private readonly HttpClient _client;

        public OrderHttpClient(HttpClient client)
        {
            this._client = client;
        }

        public async Task<IEnumerable<OrderViewModel>> GetMyOrdersAsync()
        => await _client.GetFromJsonAsync<IEnumerable<OrderViewModel>>("o/api/v1/orders/my");

        public async Task Checkout(ShoppingCartDto shoppingCartDto)
        {
            await _client.PostAsJsonAsync("o/api/v1/orders", shoppingCartDto);
        }
    }
}
