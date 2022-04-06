using System.ComponentModel.DataAnnotations;

namespace MusicReviewsWebsite.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
