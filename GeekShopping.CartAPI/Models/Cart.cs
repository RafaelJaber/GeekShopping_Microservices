using Microsoft.EntityFrameworkCore.Query;

namespace GeekShopping.CartAPI.Models {
    public class Cart {
        public CartHeader CartHeader { get; set; }
        public IIncludableQueryable<CartDetail, Product> CartDetails { get; set; }
    }
}
