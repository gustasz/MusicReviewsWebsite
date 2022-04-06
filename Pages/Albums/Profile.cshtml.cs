using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages.Albums
{
    public class ProfileModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileModel(MusicReviewsWebsite.Data.MusicContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public Album Album { get; set; }
        public List<Review> Reviews { get; set; }
        public Review UserReview { get; set; }
        public IList<Genre> TopGenres { get; set; }
        public async Task<ActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Album = await _context.Album
                .Include(a => a.Artists)
                .Include(r => r.Reviews).ThenInclude(u => u.ApplicationUser)
                .Include(g => g.GenreSuggestions).ThenInclude(g => g.Genre)
                .Include(g => g.GenreSuggestions).ThenInclude(a => a.ApplicationUser)
                .AsNoTracking()
                .SingleOrDefaultAsync(a => a.Id == id);

            if (Album == null)
            {
                return NotFound();
            }

            TopGenres = new List<Genre>();
            var genresSuggested = new List<GenreSuggestionVM>();
            Album.GenreSuggestions.GroupBy(g => g.Genre).ToList().ForEach(grp =>
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
            var genreList = genresSuggested.OrderByDescending(a => a.UsersFor.Count - a.UsersAgainst.Count).Take(2)
                .Select(x => new { x.Genre, x.GenreId }).ToList();
            genreList.ForEach(grp =>
            {
                TopGenres.Add(new Genre
                {
                    Name = grp.Genre,
                    Id = grp.GenreId,
                });
            });

            Reviews = Album.Reviews;

            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
                UserReview = Album.Reviews
                    .SingleOrDefault(r => r.ApplicationUser.Id == _userManager.GetUserId(User));
            Reviews.Remove(UserReview);

            return Page();
        }
    }
}
