using GeekShopping.ProductApi.Repository.IRepository;
using GeekShopping.ProductApi.Models.Context;
using GeekShopping.ProductApi.Models.ValueObjects;
using GeekShopping.ProductApi.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductApi.Repository {
    using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
    using System.Data.Common;


    public class ProductRepository : IProductRepository {
        private readonly SqlContext _context;
        private readonly IMapper _mapper;
        
        public ProductRepository(SqlContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        
        public async Task<IEnumerable<ProductVo>> FindAllAsync()
        {
            List<Product> products = await _context.Products.ToListAsync();
            return _mapper.Map<List<ProductVo>>(products);
        }
        
        public async Task<ProductVo> FindByIdAsync(long id)
        {
            Product product = await _context.Products
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync() ?? new Product();
            return _mapper.Map<ProductVo>(product);
        }
        
        public async Task<ProductVo> CrateAsync(ProductVo vo)
        {
            Product product = _mapper.Map<Product>(vo);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductVo>(product);
        }
        
        public async Task<ProductVo> UpdateAsync(ProductVo vo)
        {
            Product product = _mapper.Map<Product>(vo);
            try{
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return _mapper.Map<ProductVo>(product);
            }
            catch (Exception){
                return new ProductVo();
            }
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            try{
                Product product = await _context.Products
                    .Where(p => p.Id == id)
                    .FirstOrDefaultAsync() ?? new Product();
                if (product.Id <= 0) return false;
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbException){
                return false;
            }
        }
    }
}
