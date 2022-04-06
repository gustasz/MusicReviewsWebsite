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

namespace MusicReviewsWebsite.Pages.Albums
{
    [Authorize(Roles = "Admin,Moderator")]
    public class DetailsModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;

        public DetailsModel(MusicReviewsWebsite.Data.MusicContext context)
        {
            _context = context;
        }

        public Album Album { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Album = await _context.Album
                .Include(a => a.Artists)
                .AsNoTracking()
                .SingleOrDefaultAsync(a => a.Id == id);

            if (Album == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
