using AutoMapper;

namespace GeekShopping.OrderApi.Config {
    public class MappingConfig {

        public static MapperConfiguration RegisterMaps()
        {
            MapperConfiguration mapperConfig = new MapperConfiguration(config => {
                
            });
            return mapperConfig;
        }
        
    }
}
