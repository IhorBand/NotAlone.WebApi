using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotAlone.WebApi.ModelsDB
{
    [Table("T_Video_Playlist")]
    public class VideoPlaylist
    {
        [Column("PK_Video_Playlist")]
        [Key]
        public Guid Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("CreatedDateUTC")]
        public DateTime CreatedDateUtc { get; set; }
    }
}
