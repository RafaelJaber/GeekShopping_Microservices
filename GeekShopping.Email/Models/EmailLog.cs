using GeekShopping.Email.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.Email.Models {
    [Table("TB_EmailLog")]
    public class EmailLog : BaseEntity{
        [Column("Email")]
        public string? Email { get; set; }
        
        [Column("Log")]
        public string? Log { get; set; }

        [Column("SentDate")]
        public DateTime SentDate { get; set; } = DateTime.Now;
    }
}
