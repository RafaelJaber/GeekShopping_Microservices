namespace GeekShopping.CartAPI.Models.ValueObjects {
    public class CartDetailVo {
        public long Id { get; set; }
        public long CartHeaderId { get; set; }
        public CartHeaderVo CartHeader { get; set; }
        public long ProductId { get; set; }
        public ProductVo Product { get; set; }
        public int Count { get; set; }
    }
}
