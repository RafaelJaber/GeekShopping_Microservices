namespace GeekShopping.Web.Models {
    public class CouponViewModel {
        public long Id { get; set; }
        public string? CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public string? UserId { get; set; }
    }
}
