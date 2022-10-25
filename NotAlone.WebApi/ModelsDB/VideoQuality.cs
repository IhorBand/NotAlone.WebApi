using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NotAlone.WebApi.ModelsDB
{
    [Table("T_Video_Quality")]
    public class VideoQuality
    {
        [Column("PK_Video_Quality")]
        [Key]
        public Guid Id { get; set; }

        [Column("Url")]
        public string Url { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("FK_Video")]
        public Guid VideoId { get; set; }

        [Column("CreatedDateUTC")]
        public DateTime CreatedDateUtc { get; set; }
    }
}
