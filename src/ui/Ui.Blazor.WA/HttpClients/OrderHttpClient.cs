using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Ui.Blazor.WA.Models.ViewModels;
using System.Net.Http.Json;

namespace Ui.Blazor.WA.HttpClients
{
    public class OrderHttpClient
    {
        private readonly HttpClient _client;

        public OrderHttpClient(HttpClient client)
        {
            this._client = client;
        }

        public async Task<IEnumerable<ProductViewModel>> GetMyOrdersAsync()
        {
            return await _client.GetFromJsonAsync<IEnumerable<ProductViewModel>>("o/api/v1/orders/my");
        }
    }
}
