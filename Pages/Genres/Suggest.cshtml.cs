using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicReviewsWebsite.Data;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages.Genres
{
    [Authorize]
    public class SuggestModel : PageModel
    {
        private readonly MusicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public SuggestModel(MusicContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<GenreSuggestionVM> GenresSuggested { get; set; }
        public Album Album { get; set; }
        public async Task OnGetAsync(int album_id)
        {
            var album = await _context.Album
                .Include(g => g.GenreSuggestions).ThenInclude(g => g.Genre)
                .Include(g => g.GenreSuggestions).ThenInclude(u => u.ApplicationUser)
                .AsNoTracking().SingleOrDefaultAsync(a => a.Id == album_id);
            Album = album;

            var genresSuggested = new List<GenreSuggestionVM>();
            var genres = album.GenreSuggestions.ToList().GroupBy(g => g.Genre.Id);
            foreach (var grp in genres)
            {
                var genreVM = new GenreSuggestionVM();
                genreVM.Genre = grp.First().Genre.Name;
                genreVM.GenreId = grp.Key;
                genreVM.Description = grp.First().Genre.Description;
                foreach (var item in grp)
                {
                    if (item.IsFor)
                        genreVM.UsersFor.Add(item.ApplicationUser.Name);
                    else
                        genreVM.UsersAgainst.Add(item.ApplicationUser.Name);
                }
                genresSuggested.Add(genreVM);
            }
            GenresSuggested = genresSuggested.OrderByDescending(a => a.UsersFor.Count - a.UsersAgainst.Count).ToList();
            var suggestedGenreIds = genresSuggested.Select(g => g.GenreId);

            ViewData["GenreId"] = _context.Genre.Where(g => !suggestedGenreIds.Contains(g.Id)).Select(g =>
                                            new SelectListItem
                                            {
                                                Value = g.Id.ToString(),
                                                Text = g.Name
                                            }).ToList();
        }

        public async Task<IActionResult> OnPostVoteAsync(int album_id, int genre_id, bool isFor)
        {
            var user = await _userManager.GetUserAsync(User);


            var album = await _context.Album
                .Include(g => g.GenreSuggestions).ThenInclude(g => g.Genre)
                .Include(g => g.GenreSuggestions).ThenInclude(u => u.ApplicationUser)
                .SingleOrDefaultAsync(a => a.Id == album_id);
            var allGenreVotes = album.GenreSuggestions.Where(g => g.Genre.Id == genre_id).ToList();
            var genre = await _context.Genre.FindAsync(genre_id);

            if (album.GenreSuggestions == null)
            {
                album.GenreSuggestions = new List<GenreSuggestion>();
            }

            if (allGenreVotes.Any(a => a.ApplicationUser.Id == user.Id)) // check, if the user has already voted on that genre
            {
                var userVote = allGenreVotes.FirstOrDefault(a => a.ApplicationUser.Id == user.Id);
                userVote.IsFor = isFor;
            }
            else
            {
                album.GenreSuggestions.Add(new GenreSuggestion
                {
                    Album = album,
                    Genre = genre,
                    ApplicationUser = user,
                    IsFor = isFor
                });
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("/Genres/Suggest", new { album_id = album.Id });
        }

        public async Task<IActionResult> OnPostUndoAsync(int album_id, int genre_id, string userName)
        {
            // check if user actually has written a review
            var user = await _userManager.GetUserAsync(User);

            if (userName != null)
            {
                if (!(await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Moderator")))
                {
                    return NotFound();
                }
                user = await _context.Users.FirstOrDefaultAsync(u => u.Name == userName);
                if (user == null)
                {
                    return NotFound();
                }
            }

            var album = await _context.Album
                .Include(g => g.GenreSuggestions).ThenInclude(g => g.Genre)
                .Include(g => g.GenreSuggestions).ThenInclude(u => u.ApplicationUser)
                .SingleOrDefaultAsync(a => a.Id == album_id);
            var allGenreVotes = album.GenreSuggestions.Where(g => g.Genre.Id == genre_id).ToList();

            var userVote = allGenreVotes.Where(v => v.ApplicationUser.Id == user.Id).SingleOrDefault();

            album.GenreSuggestions.Remove(userVote);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Genres/Suggest", new { album_id = album.Id });
        }
    }
}
