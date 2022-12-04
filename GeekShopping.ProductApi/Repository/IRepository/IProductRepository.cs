using GeekShopping.ProductApi.Models.ValueObjects;

namespace GeekShopping.ProductApi.Repository.IRepository {

    public interface IProductRepository {
        Task<IEnumerable<ProductVo>> FindAllAsync();
        Task<ProductVo> FindByIdAsync(long id);
        Task<ProductVo> CrateAsync(ProductVo vo);
        Task<ProductVo> UpdateAsync(ProductVo vo);
        Task<bool> DeleteAsync(long id);
    }
}
