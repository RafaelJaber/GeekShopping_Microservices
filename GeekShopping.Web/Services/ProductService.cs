using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;

namespace GeekShopping.Web.Services {

    public class ProductService : IProductService {

        private readonly HttpClient _client;
        private const string BasePath = "api/v1/product";

        public ProductService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<Product>?> FindAllProductsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(BasePath);
            return await response.ReadContentAs<List<Product>>();
        }

        public async Task<Product?> FindProductByIdAsync(long id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{BasePath}/{id}");
            return await response.ReadContentAs<Product>();
        }

        public async Task<Product?> CrateProductAsync(Product model)
        {
            HttpResponseMessage response = await _client.PostAsJson(BasePath, model);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<Product>();
            else throw new Exception("Something went wrong when calling API");
        }
        public async Task<Product?> UpdateProductAsync(Product model)
        {
            HttpResponseMessage response = await _client.PutAsJson(BasePath, model);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<Product>();
            else throw new Exception("Something went wrong when calling API");
        }

        public async Task<bool> DeleteProductAsync(long id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"{BasePath}/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<bool>();
            else throw new Exception("Something went wrong when calling API");
        }
    }
}
