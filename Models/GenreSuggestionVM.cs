namespace MusicReviewsWebsite.Models
{
    public class GenreSuggestionVM
    {
        public string Genre { get; set; }
        public int GenreId { get; set; }
        public string Description { get; set; }
        public List<string> UsersFor { get; set; } = new List<string>();
        public List<string> UsersAgainst { get; set; } = new List<string>();
    }
}
