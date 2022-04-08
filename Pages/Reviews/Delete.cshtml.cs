using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class DeleteModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteModel(MusicReviewsWebsite.Data.MusicContext context, UserManager<ApplicationUser> userManager)
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

            var albumReviewList = await _context.Review.Include(a => a.Album)
                                                       .Include(u => u.ApplicationUser)
                                                       .Where(a => a.Album.Id == id)
                                                       .AsNoTracking()
                                                       .ToListAsync();

            var user = await _userManager.GetUserAsync(User);
            Review = albumReviewList.SingleOrDefault(a => a.ApplicationUser.Id == user.Id);
            if (Review == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            Review = await _context.Review // get review by user id
                .Include(a => a.Album) 
                .Include(u => u.ApplicationUser)
                .Where(a => a.Album.Id == id)
                .FirstOrDefaultAsync(a => a.ApplicationUser.Id == user.Id);

            if (Review != null)
            {
                _context.Review.Remove(Review);
                var album = await _context.Album.Include(a => a.Reviews).FirstOrDefaultAsync(a => a.Id == Review.Album.Id);
                album.ReviewCount = album.Reviews.Count() - 1;
                album.AverageRating = album.ReviewCount != 0 ? (((album.Reviews.Sum(x => x.Rating) - Review.Rating) * 100) / album.ReviewCount) : 0; // avoid divide by 0
                _context.Album.Update(album);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Albums/Profile", new { id = Review.Album.Id });
        }
    }
}
