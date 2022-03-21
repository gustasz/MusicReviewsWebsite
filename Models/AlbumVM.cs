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
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        [DisplayName("Cover")]
        public IFormFile FormFile { get; set; }
    }
}
