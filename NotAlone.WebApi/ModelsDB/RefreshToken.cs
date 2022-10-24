using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotAlone.WebApi.ModelsDB
{
    [Table("T_Refresh_Token")]
    public class RefreshToken
    {
        [Column("PK_Refresh_Token")]
        [Key]
        public Guid Id { get; set; }

        [Column("FK_User")]
        public Guid UserId { get; set; }

        [Column("Refresh_Token")]
        public Guid Token { get; set; }

        [Column("ExpiredDateUTC")]
        public DateTime ExpiredDateUTC { get; set; }

        [Column("CreatedDateUTC")]
        public DateTime CreatedDateUTC { get; set; } 
    }
}
