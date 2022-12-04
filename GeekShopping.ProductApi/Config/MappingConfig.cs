using AutoMapper;
using GeekShopping.ProductApi.Models;
using GeekShopping.ProductApi.Models.ValueObjects;

namespace GeekShopping.ProductApi.Config {

    public class MappingConfig {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<ProductVo, Product>();
                config.CreateMap<Product, ProductVo>();
            });
            return mappingConfig;
        }
    }
}
