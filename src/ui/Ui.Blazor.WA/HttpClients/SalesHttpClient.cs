using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Ui.Blazor.WA.Models.Dto;

namespace Ui.Blazor.WA.HttpClients
{
    public class SalesHttpClient
    {
        private readonly HttpClient _client;

        public SalesHttpClient(HttpClient client)
        {
            this._client = client;
        }
        
        //public async Task<IEnumerable<OrderViewModel>> GetSalesAsync()
        //=> await _client.GetFromJsonAsync<IEnumerable<OrderViewModel>>("o/api/v1/sales");

        public async Task SaleAsync(SalesDto salesDto)
        => await _client.PostAsJsonAsync("s/api/v1/sales", salesDto);
    }
}
