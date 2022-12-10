namespace GeekShopping.CouponApi.Models.ValueObjects {
    public class CouponVo {
        public Guid Id { get; set; }
        public string? CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ExpireDate { get; set; } = DateTime.Now.AddDays(10);
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public Guid UserId { get; set; }
    }
}
