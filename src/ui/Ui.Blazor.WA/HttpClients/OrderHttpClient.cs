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

        public async Task<IEnumerable<MyOrderViewModel>> GetMyOrdersAsync()
        => await _client.GetFromJsonAsync<IEnumerable<MyOrderViewModel>>("o/api/v1/orders/my");

        public async Task<IEnumerable<OrderToSaleViewModel>> GetOrdersToSaleAsync()
        => await _client.GetFromJsonAsync<IEnumerable<OrderToSaleViewModel>>("o/api/v1/orders");

        public async Task CheckoutAsync(ShoppingCartDto shoppingCartDto)
        => await _client.PostAsJsonAsync("o/api/v1/orders", shoppingCartDto);
    }
}
