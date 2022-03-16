using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicReviewsWebsite.Data;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages.Albums
{
    public class EditModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;

        public EditModel(MusicReviewsWebsite.Data.MusicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Album Album { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Album = await _context.Album
                .Include(a => a.Artist).FirstOrDefaultAsync(m => m.Id == id);

            if (Album == null)
            {
                return NotFound();
            }
           ViewData["ArtistId"] = _context.Artist.Select(a =>
                                                        new SelectListItem
                                                        {
                                                            Value = a.Id.ToString(),
                                                            Text = a.Name
                                                        }).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var albumToUpdate = await _context.Album.FindAsync(id);

            if (albumToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Album>(
                albumToUpdate,
                "album",
                a => a.Name, a => a.ReleaseDate, a => a.CoverPath, a => a.ArtistId))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }

        private bool AlbumExists(int id)
        {
            return _context.Album.Any(e => e.Id == id);
        }
    }
}
