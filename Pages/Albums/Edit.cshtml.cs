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

namespace MusicReviewsWebsite.Pages.Albums
{
    [Authorize(Roles = "Admin,Moderator")]
    public class EditModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;
        private readonly IWebHostEnvironment _environment;

        private string[] permittedExtensions = { ".png", ".jpg", ".jpeg", ".png" };
        public EditModel(MusicReviewsWebsite.Data.MusicContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public AlbumVM AlbumVM { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Album album = await _context.Album
                .Include(a => a.Artists).FirstOrDefaultAsync(m => m.Id == id);

            if (album == null)
            {
                return NotFound();
            }

            AlbumVM = new AlbumVM
            {
                Id = album.Id,
                Name = album.Name,
                ReleaseDate = album.ReleaseDate,
                CoverPath = album.CoverPath,
                Artists = album.Artists
            };

            // show already selected artists
            AlbumVM.ArtistIds = new string[album.Artists.Count];
            for(int i = 0; i < album.Artists.Count; i++)
            {
                AlbumVM.ArtistIds[i] = album.Artists[i].Id.ToString();
            }

           ViewData["ArtistId"] = _context.Artist.Select(a =>
                                                        new SelectListItem
                                                        {
                                                            Value = a.Id.ToString(),
                                                            Text = a.Name
                                                        }).ToList().OrderBy(x => x.Text);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var albumToUpdate = await _context.Album.Include(a => a.Artists).SingleOrDefaultAsync(a => a.Id == id);

            if (!ModelState.IsValid)
            {
                ViewData["ArtistId"] = _context.Artist.Select(a =>
                                             new SelectListItem
                                             {
                                                 Value = a.Id.ToString(),
                                                 Text = a.Name
                                             }).ToList().OrderBy(x => x.Text);
                return Page();
            }
            
            var formFile = AlbumVM.FormFile;
            if (formFile != null && formFile.Length > 0)
            {
                var ext = Path.GetExtension(formFile.FileName).ToLowerInvariant();

                if (!string.IsNullOrEmpty(ext) && permittedExtensions.Contains(ext))
                {
                    if (albumToUpdate.CoverPath != Path.Combine("Images", "Temp", "defaultAlbumPicture.png") && !string.IsNullOrEmpty(albumToUpdate.CoverPath))
                    {
                        var fullPath = Path.Combine(_environment.WebRootPath, albumToUpdate.CoverPath);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }

                    var fileName = Path.GetRandomFileName() + ext;
                    var filePath = Path.Combine("Images",
                        "Album Covers",
                        fileName);
                    var fullFilePath = Path.Combine(_environment.WebRootPath, filePath);

                    using (var stream = System.IO.File.Create(fullFilePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    albumToUpdate.CoverPath = filePath;
                }
            }

            albumToUpdate.Name = AlbumVM.Name;
            albumToUpdate.ReleaseDate = AlbumVM.ReleaseDate;

            albumToUpdate.Artists = new List<Artist>(); // remove all old selected artists
            foreach (var itemId in AlbumVM.ArtistIds)
            {
                var artist = await _context.Artist.FindAsync(Int32.Parse(itemId));
                albumToUpdate.Artists.Add(artist);
            }

            _context.Update(albumToUpdate);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
