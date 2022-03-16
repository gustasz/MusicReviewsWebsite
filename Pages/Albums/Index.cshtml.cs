using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicReviewsWebsite.Data;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages.Albums
{
    public class IndexModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;

        public IndexModel(MusicReviewsWebsite.Data.MusicContext context)
        {
            _context = context;
        }

        public IList<Album> Album { get;set; }

        public async Task OnGetAsync()
        {
            Album = await _context.Album
                .Include(a => a.Artist).ToListAsync();
        }
    }
}
