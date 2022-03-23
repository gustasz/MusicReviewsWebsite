using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicReviewsWebsite.Data;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages.Artists
{
    [Authorize(Roles = "Admin,Moderator")]
    public class CreateModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;

        public CreateModel(MusicReviewsWebsite.Data.MusicContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Artist Artist { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var emptyArtist = new Artist();

            if (await TryUpdateModelAsync<Artist>(
                emptyArtist,
                "artist", // Prefix for form value.
                a => a.Name, a => a.AlsoKnownAs, a => a.BirthDate))
            {
                _context.Artist.Add(emptyArtist);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            return Page();

        }
    }
}
