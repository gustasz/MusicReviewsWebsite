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
        private readonly IWebHostEnvironment _environment;

        private string[] permittedExtensions = { ".png", ".jpg", ".jpeg", ".png"};

        public CreateModel(MusicReviewsWebsite.Data.MusicContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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
        public AlbumVM AlbumVM { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string filePath = Path.Combine("Images", "Temp", "defaultAlbumPicture.png");
            var formFile = AlbumVM.FormFile;
            if (formFile != null)
            {
                var ext = Path.GetExtension(formFile.FileName).ToLowerInvariant();

                if (!string.IsNullOrEmpty(ext) && permittedExtensions.Contains(ext))
                {
                    var fileName = Path.GetRandomFileName() + ext;
                    filePath = Path.Combine("Images",
                        "Album Covers",
                        fileName);
                    var fullFilePath = Path.Combine(_environment.WebRootPath, filePath);


                    using (var stream = System.IO.File.Create(fullFilePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            var newAlbum = new Album
            {
                Name = AlbumVM.Name,
                ReleaseDate = AlbumVM.ReleaseDate,
                CoverPath = filePath
            };

            // assign artists
            for (int i = 0; i < AlbumVM.ArtistIds.Count(); i++)
            {
                var artist = _context.Artist.SingleOrDefault(a => a.Id == Int32.Parse(AlbumVM.ArtistIds[i]));
                newAlbum.Artists.Add(artist);
            }

            _context.Add(newAlbum);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
