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
    public class DeleteModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;
        private readonly ILogger<DeleteModel> _logger;
        private readonly IWebHostEnvironment _environment;

        public DeleteModel(MusicReviewsWebsite.Data.MusicContext context,
                            ILogger<DeleteModel> logger,
                            IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
        }

        [BindProperty]
        public Album Album { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Album = await _context.Album
                .Include(a => a.Artists)
                .AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);

            if (Album == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = String.Format("Delete {Id} failed. Try again", id);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album.FindAsync(id);

            if (album == null)
            {
                return NotFound();
            }

            try
            {
                var reviewList = _context.Review.Where(a => a.Album.Id == album.Id).ToList();
                _context.Review.RemoveRange(reviewList);
                _context.Album.Remove(album);
                await _context.SaveChangesAsync();
                if (album.CoverPath != Path.Combine("Images", "Temp", "defaultAlbumPicture.png") && !string.IsNullOrEmpty(album.CoverPath))
                {
                    var fullPath = Path.Combine(_environment.WebRootPath, album.CoverPath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ErrorMessage);

                return RedirectToAction("./Delete",
                                    new { id, saveChangesError = true });
            }
        }
    }
}
