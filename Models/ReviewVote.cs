using System.ComponentModel.DataAnnotations;

namespace MusicReviewsWebsite.Models
{
    public class ReviewVote
    {
        public int Id { get; set; }
        [Required]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public Review Review { get; set; }
        public bool IsFor { get; set; }
    }
}
