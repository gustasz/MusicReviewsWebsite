using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicReviewsWebsite.Models
{
    public class AlbumVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }
        public string CoverPath { get; set; }

        [DisplayName("Artist")]
        public string[] ArtistIds { get; set; }
        public List<Artist> Artists { get; set; } = new List<Artist>();

        [DisplayName("Cover")]
        public IFormFile FormFile { get; set; }
    }
}
