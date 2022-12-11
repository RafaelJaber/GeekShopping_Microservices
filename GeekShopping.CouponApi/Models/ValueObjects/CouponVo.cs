namespace GeekShopping.CouponApi.Models.ValueObjects {
    public class CouponVo {
        public long Id { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public string UserId { get; set; }
    }
}
