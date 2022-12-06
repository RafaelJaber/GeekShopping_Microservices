using GeekShopping.CartAPI.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.CartAPI.Models {
    [Table("CartHeader")]
    public class CartHeader : BaseEntity{
        [Column("UserId")]
        public string UserId { get; set; }
        [Column("CuponCode")]
        public string CuponCode { get; set; }
    }
}
