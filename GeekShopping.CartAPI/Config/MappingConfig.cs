using AutoMapper;
using GeekShopping.CartAPI.Models;
using GeekShopping.CartAPI.Models.ValueObjects;

namespace GeekShopping.CartAPI.Config {
    public class MappingConfig {
        public static MapperConfiguration RegisterMaps()
        {
            MapperConfiguration mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<CartVo, Cart>();
                config.CreateMap<Cart, CartVo>();
                config.CreateMap<ProductVo, Product>().ReverseMap();
                config.CreateMap<CartHeaderVo, CartHeader>().ReverseMap();
                config.CreateMap<CartDetailVo, CartDetail>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
