using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicReviewsWebsite.Models;
using System.ComponentModel;
using System.Linq;

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
        public List<Review> ReviewsWithText { get; set; }
        public Review UserReview { get; set; }
        [DisplayName("Genres")]
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
                .Include(r => r.Reviews).ThenInclude(v => v.Votes)
                .Include(g => g.GenreSuggestions).ThenInclude(g => g.Genre)
                .Include(g => g.GenreSuggestions).ThenInclude(a => a.ApplicationUser)
                .AsNoTracking().SingleOrDefaultAsync(a => a.Id == id);

            if (Album == null)
            {
                return NotFound();
            }

            //get top album genres
            var genresSuggested = new List<GenreSuggestionVM>();
            Album.GenreSuggestions.GroupBy(g => g.Genre.Id).ToList().ForEach(grp =>
            {
                var genreVM = new GenreSuggestionVM();
                genreVM.Genre = grp.First().Genre.Name;
                genreVM.GenreId = grp.Key;
                foreach (var item in grp)
                {
                    if (item.IsFor)
                        genreVM.UsersFor.Add(item.ApplicationUser.UserName);
                    else
                        genreVM.UsersAgainst.Add(item.ApplicationUser.UserName);
                }
                genresSuggested.Add(genreVM);
            });
            var genreList = genresSuggested.OrderByDescending(a => a.UsersFor.Count - a.UsersAgainst.Count).ToList().Take(2);
            TopGenres = new List<Genre>();
            foreach (var genre in genreList)
            {
                TopGenres.Add(new Genre { Id = genre.GenreId, Name = genre.Genre });
            }
            Reviews = Album.Reviews;

            // im doing a second query, because using two thenInclude for 2nd level in the main album query made my website stuck in loading
            ReviewsWithText = await _context.Review.Where(a => a.Album.Id == Album.Id).Where(t => t.Text != null)
                .Include(a => a.ApplicationUser).Include(v => v.Votes).ThenInclude(a => a.ApplicationUser)
                .OrderByDescending(r => r.Votes.Where(i => i.IsFor == true).Count() - r.Votes.Where(i => i.IsFor == false).Count())
                .AsNoTracking().ToListAsync();

            //show review differently if its written by the current user
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                if (Album.Reviews.Any(x => x.ApplicationUser.Id == userId))
                {
                    UserReview = Album.Reviews.SingleOrDefault(r => r.ApplicationUser.Id == userId);
                    Reviews.Remove(UserReview);
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostVoteAsync(int review_id, bool isFor)
        {
            var review = await _context.Review.Include(a => a.Album).Include(a => a.ApplicationUser)
                .Include(v => v.Votes).ThenInclude(a => a.ApplicationUser).SingleOrDefaultAsync(r => r.Id == review_id);
            var currVotes = review.Votes;

            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (!isAuthenticated || review.Text == null)
            {
                return RedirectToPage("/Albums/Profile", new { id = review.Album.Id });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user.Id == review.ApplicationUser.Id)
            {
                return RedirectToPage("/Albums/Profile", new { id = review.Album.Id });
            }

            if (!currVotes.Any(a => a.ApplicationUser.Id == user.Id))
            {
                var reviewVote = new ReviewVote { ApplicationUser = user, Review = review, IsFor = isFor };
                review.Votes.Add(reviewVote);
            }
            else
            {
                currVotes.SingleOrDefault(a => a.ApplicationUser.Id == user.Id).IsFor = isFor;
            }
            await _context.SaveChangesAsync();


            return RedirectToPage("/Albums/Profile", new { id = review.Album.Id });
        }

        public async Task<IActionResult> OnPostUndoAsync(int review_id)
        {
            var review = await _context.Review.Include(a => a.Album).Include(a => a.ApplicationUser)
                .Include(v => v.Votes).ThenInclude(a => a.ApplicationUser).SingleOrDefaultAsync(r => r.Id == review_id);
            var currVotes = review.Votes;

            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (!isAuthenticated || review.Text == null)
            {
                return RedirectToPage("/Albums/Profile", new { id = review.Album.Id });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user.Id == review.ApplicationUser.Id)
            {
                return RedirectToPage("/Albums/Profile", new { id = review.Album.Id });
            }

            var userVote = currVotes.SingleOrDefault(a => a.ApplicationUser.Id == user.Id);
            review.Votes.Remove(userVote);
            await _context.SaveChangesAsync();


            return RedirectToPage("/Albums/Profile", new { id = review.Album.Id });
        }
    }
}
