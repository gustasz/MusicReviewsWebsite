using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages.Albums
{
    public class ProfileModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;

        public ProfileModel(MusicReviewsWebsite.Data.MusicContext context)
        {
            _context = context;
        }
        public Album Album { get; set; }
        public async Task<ActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Album = await _context.Album.Include(a => a.Artists).AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

            if (Album == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
