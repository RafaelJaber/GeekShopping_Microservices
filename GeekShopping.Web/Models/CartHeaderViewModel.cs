namespace GeekShopping.Web.Models {
    public class CartHeaderViewModel {
        public long Id { get; set; }
        public string? UserId { get; set; }
        public string CuponCode { get; set; }
        public double PurchaseAmount { get; set; }
        
        public double DiscountTotal { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; } 
        public DateTime DateTime { get; set; }
        public string? CardNumber { get; set; }
        public string? Cvv { get; set; }
        public string? ExpiryMothYear { get; set; }
    }
}
