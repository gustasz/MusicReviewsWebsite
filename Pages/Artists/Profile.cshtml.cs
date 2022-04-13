using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicReviewsWebsite.Data;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages.Artists
{
    public class ProfileModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;

        public ProfileModel(MusicReviewsWebsite.Data.MusicContext context)
        {
            _context = context;
        }

        public Artist Artist { get; set; }
        public List<Album> Albums { get; set; } = new List<Album>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Artist = await _context.Artist.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);

            if (Artist == null)
            {
                return NotFound();
            }

            Albums = await _context.Album.Where(a => a.Artists.Contains(Artist)).OrderByDescending(a => a.ReleaseDate).ToListAsync();

            return Page();
        }
    }
}
