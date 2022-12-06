using GeekShopping.CartAPI.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.CartAPI.Models {
    [Table("CartDetail")]
    public class CartDetail : BaseEntity {
        
        public long CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]
        public CartHeader CartHeader { get; set; }
        
        
        public long ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        
        [Column("Count")]
        public int Count { get; set; }
    }
}
