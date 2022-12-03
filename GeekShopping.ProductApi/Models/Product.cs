using GeekShopping.ProductApi.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.ProductApi.Models {
    using System.ComponentModel.DataAnnotations;

    [Table("TB_Product")]
    public class Product : BaseEntity {
        [Column("Name")]
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Column("Price")]
        [Required]
        [Range(1, 10000)]
        public decimal Price { get; set; }

        [Column("Description")]
        [StringLength(500)]
        public string Description { get; set; }

        [Column("CategoryName")]
        [StringLength(50)]
        public string Category { get; set; }

        [Column("ImageUrl")]
        [StringLength(300)]
        public string ImageUrl { get; set; }
    }
}
