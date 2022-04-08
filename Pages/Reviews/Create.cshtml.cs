using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicReviewsWebsite.Data;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages.Reviews
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly MusicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(MusicContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [BindProperty]
        public Review Review { get; set; }
        public Album Album { get; set; }
        public async Task<ActionResult> OnGet(int? id)
        {
            if (id == null)
                return NotFound();
            Album = await _context.Album.AsNoTracking().SingleOrDefaultAsync(a => a.Id == id);
            return Page();
        }
        public async Task<ActionResult> OnPostAsync(int id)
        {
            var emptyReview = new Review();
            var user = await _userManager.GetUserAsync(User);
            Review.ApplicationUser = user;
            var album = _context.Album.Include(r => r.Reviews).SingleOrDefault(m => m.Id == id);
            if(album.Reviews.Any(r => r.ApplicationUser == user) || album.Reviews == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Review>(
                emptyReview,
                "review", // Prefix for form value.
                a => a.Rating, a => a.Text))
            {
                emptyReview.ApplicationUser = user;
                emptyReview.Album = album;
                _context.Review.Add(emptyReview);

                album.ReviewCount = album.Reviews.Count();
                album.AverageRating = ((album.Reviews.Sum(x => x.Rating) * 100) / album.ReviewCount); // average is stored in 0.01 accuracy

                await _context.SaveChangesAsync();
                return RedirectToPage("/Albums/Profile", new { id = id});
            }
            return Page();
        }
    }
}
