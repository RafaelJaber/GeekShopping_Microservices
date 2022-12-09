namespace GeekShopping.CartAPI.Models.ValueObjects {
    public class CartVo {
        public CartHeader CartHeader { get; set; }
        public IEnumerable<CartDetail> CartDetails { get; set; }
    }
}
