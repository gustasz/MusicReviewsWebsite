namespace MusicReviewsWebsite.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AlsoKnownAs { get; set; }
        public DateTime BirthDate { get; set; }

        public List<Album> Albums { get; set; }
    }
}
