using GeekShopping.Web.Models;


namespace GeekShopping.Web.Services.IServices {

    public interface IProductService {
        Task<IEnumerable<Product>?> FindAllProductsAsync(string token);
        Task<Product?> FindProductByIdAsync(long id, string token);
        Task<Product?> CrateProductAsync(Product model, string token);
        Task<Product?> UpdateProductAsync(Product model, string token);
        Task<bool> DeleteProductAsync(long id, string token);
    }
}
