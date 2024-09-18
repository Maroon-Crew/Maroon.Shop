using Maroon.Shop.Api.Requests;
using Maroon.Shop.Api.Responses;
using System.Net.Http.Json;

namespace Maroon.Shop.Api.Client
{
    public class ProductClient
    {
        private readonly HttpClient _httpClient;

        public ProductClient(HttpClient httpClient) 
        { 
            _httpClient = httpClient;
        }

        public async Task<ProductResponse?> GetByIdAsync(GetProductRequest getProductRequest)
        {
            return await _httpClient.GetFromJsonAsync<ProductResponse>($"Product/GetById/{getProductRequest.ProductId}");
        }

        public async Task<ProductResponse?> GetById2Async(GetProductRequest getProductRequest)
        {
            var response = await _httpClient.GetAsync($"Product/GetById/{getProductRequest.ProductId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductResponse>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                // throw
                response.EnsureSuccessStatusCode();
            }

            return null;
        }

        public async Task<ProductResponse?> GetAsync(string urlFriendlyName)
        {
            return await _httpClient.GetFromJsonAsync<ProductResponse>($"Product/{urlFriendlyName}");
        }

        public async Task<ProductResponse?> Get2Async(string urlFriendlyName)
        {
            var response = await _httpClient.GetAsync($"Product/{urlFriendlyName}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductResponse>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                // throw
                response.EnsureSuccessStatusCode();
            }

            return null;
        }
    }
}
