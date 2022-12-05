using GeekShopping.Web.Models;


namespace GeekShopping.Web.Services.IServices {

    public interface IProductService {
        Task<IEnumerable<Product>?> FindAllProductsAsync();
        Task<Product?> FindProductByIdAsync(long id);
        Task<Product?> CrateProductAsync(Product model);
        Task<Product?> UpdateProductAsync(Product model);
        Task<bool> DeleteProductAsync(long id);
    }
}
