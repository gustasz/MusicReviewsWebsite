using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages.Albums
{
    public class SearchModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;
        private readonly IConfiguration Configuration;
        public SearchModel(MusicReviewsWebsite.Data.MusicContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }
        public PaginatedList<Album> Albums;
        public string DateSort { get; set; }
        public string RatingSort { get; set; }
        public string CurrentSort { get; set; }
        public string SearchTerm { get; set; }

        public string CustomOrder { get; set; }
        public void OnGet(string searchTerm, string sortOrder, string ratingSort, int? pageIndex, string customOrder)
        {
            List<Album> albumList = new List<Album>();
            if (customOrder != null)
            {
                CustomOrder = customOrder;
                switch (customOrder)
                {
                    case "Best":
                        albumList = _context.Album.AsNoTracking().Include(a => a.Artists).OrderByDescending(a => a.AverageRating).Take(50).ToList();
                        break;
                    case "Popular":
                        albumList = _context.Album.AsNoTracking().Include(a => a.Artists).OrderByDescending(a => a.ReviewCount).Take(50).ToList();
                        break;
                }
                int i = 1;
                foreach(var album in albumList)
                {
                    album.Name = $"{i}. {album.Name}";
                    i++;
                }
            }
            else
            {
                CurrentSort = sortOrder;

                RatingSort = sortOrder == "Rating" ? "rating_desc" : "Rating";
                DateSort = sortOrder == "Date" ? "date_desc" : "Date";

                SearchTerm = searchTerm;
                List<Album> albumsByName = _context.Album.AsNoTracking().Include(a => a.Artists).Where(a => a.Name.Contains(searchTerm)).ToList();
                var artistList = _context.Artist.AsNoTracking().Include(a => a.Albums).Where(a => a.Name.Contains(searchTerm)).ToList();

                List<Album> artistAlbums = new List<Album>();
                foreach (var artist in artistList)
                {
                    artistAlbums.AddRange(artist.Albums);
                };
                albumsByName.AddRange(artistAlbums);

                albumList = albumsByName.GroupBy(a => a.Name).Select(m => m.First()).ToList();

                switch (sortOrder)
                {
                    case "Date":
                        albumList = albumList.OrderBy(a => a.ReleaseDate).ToList();
                        break;
                    case "date_desc":
                        albumList = albumList.OrderByDescending(a => a.ReleaseDate).ToList();
                        break;
                    case "rating_desc":
                        albumList = albumList.OrderByDescending(a => a.AverageRating).ToList();
                        break;
                    case "Rating":
                        albumList = albumList.OrderBy(a => a.AverageRating).ToList();
                        break;
                    default:
                        albumList = albumList.OrderBy(a => a.Name).ToList();
                        break;
                }
            }
            var pageSize = Configuration.GetValue("PageSize", 5);
            Albums = PaginatedList<Album>.Create(
                albumList, pageIndex ?? 1, pageSize);
        }
    }
}
