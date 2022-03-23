using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicReviewsWebsite.Models
{
    public class Album
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }
        [DisplayName("Average Rating")]
        public int AverageRating { get; set; }
        [DisplayName("Review Count")]
        public int ReviewCount { get; set; }
        [DisplayName("Cover Path")]
        public string CoverPath { get; set; }

        public List<Artist> Artists { get; set; } = new List<Artist>();

        public string GetRating()
        {
            return $"{AverageRating} / 10 from {ReviewCount} reviews";
        }

        public string GetArtists()
        {
            string allArtists = string.Empty;
            foreach (Artist artist in Artists)
            {
                allArtists += $"{artist.Name} ";
            }
            return allArtists;
        }
    }
}
