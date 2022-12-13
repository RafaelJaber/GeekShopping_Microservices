using GeekShopping.OrderApi.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.OrderApi.Models {
    [Table("TB_OrderHeader")]
    public class OrderHeader : BaseEntity{
        
        [Column("UserId")]
        public string? UserId { get; set; }
        
        [Column("CuponCode")]
        public string? CuponCode { get; set; }
        
        [Column("PurchaseAmount")]
        public double PurchaseAmount { get; set; }
        
        [Column("DiscountTotal")]
        public double DiscountTotal { get; set; }
        
        [Column("FirstName")]
        public string? FirstName { get; set; }
        
        [Column("LastName")]
        public string? LastName { get; set; } 
        
        [Column("PurchaseDate")]
        public DateTime DateTime { get; set; }
        
        [Column("OrderTime")]
        public DateTime OrderTime { get; set; }
        
        [Column("Email")]
        public string? Email { get; set; }
        
        [Column("Phone")]
        public string? Phone { get; set; }
        
        [Column("CardNumber")]
        public string? CardNumber { get; set; }
        
        [Column("Cvv")]
        public string? Cvv { get; set; }
        
        [Column("ExpiryMothYear")]
        public string? ExpiryMothYear { get; set; }
        
        [Column("CartTotalItems")]
        public int CartTotalItems { get; set; }
        
        [Column("OrderDetails")]
        public List<OrderDetail>? OrderDetails { get; set; }

        [Column("PaymentStatus")]
        public bool PaymentStatus { get; set; } = false;
    }
}
