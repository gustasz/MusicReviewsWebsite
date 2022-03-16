using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicReviewsWebsite.Data;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages.Albums
{
    public class CreateModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;

        public CreateModel(MusicReviewsWebsite.Data.MusicContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["ArtistId"] = _context.Artist.Select(a =>
                                                        new SelectListItem
                                                        {
                                                            Value = a.Id.ToString(),
                                                            Text = a.Name
                                                        }).ToList();
            return Page();
        }

        [BindProperty]
        public Album Album { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var emptyAlbum = new Album();

            if (await TryUpdateModelAsync<Album>(
                emptyAlbum,
                "album",
                a => a.Name, a => a.ReleaseDate, a => a.CoverPath))
            {
                _context.Album.Add(emptyAlbum);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            return Page();
        }
    }
}
