
using GeekShopping.OrderApi.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.OrderApi.Models {
    [Table("TB_OrderDetail")]
    public class OrderDetail : BaseEntity {
        
        [Column("OrderHeaderId")]
        public long OrderHeaderId { get; set; }
        
        [ForeignKey("OrderHeaderId")]
        public virtual OrderHeader? OrderHeader { get; set; }
        
        [Column("ProductId")]
        public long ProductId { get; set; }
        
        [Column("Count")]
        public int Count { get; set; }
        
        [Column("ProductName")]
        public string? ProductName { get; set; }
        
        [Column("Price")]
        public decimal Price { get; set; }
    }
}
