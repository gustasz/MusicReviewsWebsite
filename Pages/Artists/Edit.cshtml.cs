using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicReviewsWebsite.Data;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages.Artists
{
    [Authorize(Roles = "Admin,Moderator")]
    public class EditModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;

        public EditModel(MusicReviewsWebsite.Data.MusicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Artist Artist { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Artist = await _context.Artist
                .AsNoTracking().SingleOrDefaultAsync(a => a.Id == id);

            if (Artist == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var artistToUpdate = await _context.Artist.FindAsync(id);

            if (artistToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Artist>(
                artistToUpdate,
                "artist",
                a => a.Name, a => a.AlsoKnownAs, a => a.BirthDate))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
