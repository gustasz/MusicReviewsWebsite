using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, MusicReviewsWebsite.Data.MusicContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IList<Album> Albums { get; set; }

        public async Task OnGetAsync()
        {
            Albums = await _context.Album.Include(a => a.Artist).OrderByDescending(a => a.ReleaseDate).Take(8).ToListAsync();
        }
    }
}