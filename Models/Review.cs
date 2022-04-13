using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MusicReviewsWebsite.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Range(1,10)]
        public int Rating { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public Album Album { get; set; }
        public List<ReviewVote> Votes { get; set; } = new List<ReviewVote>();

        public string GetRating()
        {
            return $"{Rating}/10";
        }
    }
}
