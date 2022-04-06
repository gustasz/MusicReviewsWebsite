using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicReviewsWebsite.Data;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages.Artists
{
    [Authorize(Roles = "Admin,Moderator")]
    public class DetailsModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;

        public DetailsModel(MusicReviewsWebsite.Data.MusicContext context)
        {
            _context = context;
        }

        public Artist Artist { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Artist = await _context.Artist
                .Include(a => a.Albums)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Artist == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
