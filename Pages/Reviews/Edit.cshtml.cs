using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicReviewsWebsite.Data;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages.Reviews
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(MusicReviewsWebsite.Data.MusicContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Review Review { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albumReviewList = await _context.Review.Include(a => a.ApplicationUser)
                .Include(a => a.Album)
                .Where(m => m.Album.Id == id)
                .AsNoTracking()
                .ToListAsync();
            var user = await _userManager.GetUserAsync(User);
            Review = albumReviewList.SingleOrDefault(r => r.ApplicationUser.Id == user.Id);

            if (Review == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var reviewToUpdate = await _context.Review // get review by user id 
                                                       .Where(a => a.Album.Id == id)
                                                       .FirstOrDefaultAsync(a => a.ApplicationUser.Id == user.Id);
            if(reviewToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Review>(
                reviewToUpdate,
                "review",
                r => r.Text, r => r.Rating))
            {
                //_context.Review.Add(reviewToUpdate);
                var album = await _context.Album.Include(a => a.Reviews).SingleOrDefaultAsync(a => a.Id == Review.Album.Id);
                album.Reviews.Add(reviewToUpdate);
                album.AverageRating = (album.Reviews.Sum(x => x.Rating) - Review.Rating) / album.ReviewCount;

                await _context.SaveChangesAsync();
                return RedirectToPage("/Albums/Profile", new { id = Review.Album.Id });
            }
            return Page();
        }
    }
}
