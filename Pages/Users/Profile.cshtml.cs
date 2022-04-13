using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages.Users
{
    public class ProfileModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;

        public ProfileModel(MusicReviewsWebsite.Data.MusicContext context)
        {
            _context = context;
        }

        public ApplicationUser ApplicationUser { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>();

        public async Task<IActionResult> OnGetAsync(string name)
        {
            ApplicationUser = await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.UserName == name);
            if (ApplicationUser == null)
            {
                return NotFound();
            }
            Reviews = await _context.Review.Where(a => a.ApplicationUser.Id == ApplicationUser.Id)
                .Include(a => a.Album).OrderByDescending(d => d.CreatedDate).ToListAsync();

            return Page();
        }
    }
}
