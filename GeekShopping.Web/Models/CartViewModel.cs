namespace GeekShopping.Web.Models {
    public class CartViewModel {
        public  CartHeaderViewModel CartHeaderViewModel { get; set; }
        public IEnumerable<CartDetailViewModel> CartDetails { get; set; }
    }
}
