using System.ComponentModel.DataAnnotations;

namespace MusicReviewsWebsite.Models
{
    public class GenreSuggestion
    {
        public int Id { get; set; }
        [Required]
        public Genre Genre { get; set; }
        [Required]
        public Album Album { get; set; }
        [Required]
        public ApplicationUser ApplicationUser { get; set; }
        public bool IsFor { get; set; } // User can agree or disagree with the genre suggestion
    }
}
