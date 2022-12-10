using AutoMapper;
using GeekShopping.CouponApi.Models;
using GeekShopping.CouponApi.Models.ValueObjects;

namespace GeekShopping.CouponApi.Config {
    public class MappingConfig {
        public static MapperConfiguration RegisterMaps()
        {
            MapperConfiguration mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<CouponVo, Coupon>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
