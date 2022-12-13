namespace GeekShopping.OrderApi.Messages {
    public class CheckoutHeaderVo {
        public string? UserId { get; set; }
        public string? CuponCode { get; set; }
        public double PurchaseAmount { get; set; }
        
        public double DiscountTotal { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; } 
        public DateTime DateTime { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? CardNumber { get; set; }
        public string? Cvv { get; set; }
        public string? ExpiryMothYear { get; set; }
        public int CartTotalItems { get; set; }
        public IEnumerable<CartDetailVo>? CartDetails { get; set; }
    }
}
