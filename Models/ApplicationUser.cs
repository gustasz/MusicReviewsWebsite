using Microsoft.AspNetCore.Identity;

namespace MusicReviewsWebsite.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Review> Reviews { get; set; }
        public List<GenreSuggestion> GenreSuggestions { get; set; }
    }
}
