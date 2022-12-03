using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeekShopping.ProductApi.Models.Base {

    public class BaseEntity {
        [Key]
        [Column("Id")]
        public long Id { get; set; }
    }
}
