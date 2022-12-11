using AutoMapper;
using GeekShopping.CouponApi.Models;
using GeekShopping.CouponApi.Models.ValueObjects;

namespace GeekShopping.CouponApi.Config {
    public class MappingConfig {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<CouponVo, Coupon>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
