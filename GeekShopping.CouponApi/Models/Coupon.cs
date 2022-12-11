using GeekShopping.CouponApi.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GeekShopping.CouponApi.Models {
    [Table("TB_Coupon")]
    public class Coupon : BaseEntity {

        [Column("CouponCode")]
        [Required]
        [StringLength(30)]
        public string? CouponCode { get; set; }
        
        [Column("DiscountAmount")]
        [Required]
        public decimal DiscountAmount { get; set; }

        [Column("ExpiresDate")]
        public DateTime ExpireDate { get; set; } = DateTime.Now.AddDays(10);
        
        [Column("RegisterDate")]
        public DateTime RegisterDate { get; set; } = DateTime.Now;

        [Column("UserRegister")]
        public string? UserId { get; set; }
    }
}
