using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ui.Blazor.WA.Models.ViewModels;
using System.Net.Http.Json;

namespace Ui.Blazor.WA.HttpClients
{
    public class ProductHttpClient
    {
        private readonly HttpClient _client;

        public ProductHttpClient(HttpClient client)
        {
            this._client = client;
        }

        public async Task<IEnumerable<ProductViewModel>> GetProductsAsync()
        {
            return await _client.GetFromJsonAsync<IEnumerable<ProductViewModel>>("p/api/v1/products");
        }
    }
}
