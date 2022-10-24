using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotAlone.WebApi.ModelsDB
{
    [Table("T_User")]
    public class User
    {
        [Column("PK_User")]
        [Key]
        public Guid Id { get; set; }

        [Column("DisplayName")]
        public string DisplayName { get; set; }

        [Column("UserName")]
        public string UserName { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [Column("CreatedDateUTC")]
        public DateTime CreatedDateUTC { get; set; }
    }
}
