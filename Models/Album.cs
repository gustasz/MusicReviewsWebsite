namespace MusicReviewsWebsite.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public string CoverPath { get; set; }

        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
    }
}
