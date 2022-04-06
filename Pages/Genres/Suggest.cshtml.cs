using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            var album = await _context.Album.Include(g => g.GenreSuggestions).ThenInclude(g => g.Genre)
                                            .Include(g => g.GenreSuggestions).ThenInclude(u => u.ApplicationUser)
                                            .AsNoTracking()
                                            .SingleOrDefaultAsync(a => a.Id == album_id);
            Album = album;

            var genresSuggested = new List<GenreSuggestionVM>();
            album.GenreSuggestions.Where(a => a.Album.Id == album_id).GroupBy(g => g.Genre).ToList().ForEach(grp =>
            {
                var genreVM = new GenreSuggestionVM();
                genreVM.Genre = grp.Key.Name;
                genreVM.GenreId = grp.Key.Id;
                genreVM.Description = grp.Key.Description;
                foreach (var item in grp)
                {
                    if (item.IsFor)
                        genreVM.UsersFor.Add(item.ApplicationUser.UserName);
                    else
                        genreVM.UsersAgainst.Add(item.ApplicationUser.UserName);
                }
                genresSuggested.Add(genreVM);
            });
            GenresSuggested = genresSuggested.OrderByDescending(a => a.UsersFor.Count - a.UsersAgainst.Count).ToList();
        }

        public async Task<IActionResult> OnPostVoteAsync(int album_id,int genre_id,bool isFor)
        {
            var user = await _userManager.GetUserAsync(User);


            var album = await _context.Album
                .Include(g => g.GenreSuggestions).ThenInclude(g => g.Genre)
                .Include(g => g.GenreSuggestions).ThenInclude(u => u.ApplicationUser)
                .SingleOrDefaultAsync(a => a.Id == album_id);
            var allGenreVotes = album.GenreSuggestions.Where(g => g.Genre.Id == genre_id).ToList();

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
                    Genre = allGenreVotes.First().Genre,
                    ApplicationUser = user,
                    IsFor = isFor
                });
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("/Genres/Suggest", new { album_id = album.Id});
        }

        public async Task<IActionResult> OnPostUndoAsync(int album_id, int genre_id)
        {
            // check if user actually has written
            var user = await _userManager.GetUserAsync(User);
            var album = await _context.Album.Include(g => g.GenreSuggestions).ThenInclude(g => g.Genre)
                .Include(g => g.GenreSuggestions).ThenInclude(u => u.ApplicationUser)
                .FirstOrDefaultAsync(a => a.Id == album_id);
            var allGenreVotes = album.GenreSuggestions.Where(g => g.Genre.Id == genre_id).ToList();

            var userVote = allGenreVotes.Where(v => v.ApplicationUser.Id == user.Id).SingleOrDefault();

            album.GenreSuggestions.Remove(userVote);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Genres/Suggest", new { album_id = album.Id });
        }

    }
}
