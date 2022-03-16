using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Data
{
    public class MusicContext : DbContext
    {
        public MusicContext (DbContextOptions<MusicContext> options)
            : base(options)
        {
        }

        public DbSet<MusicReviewsWebsite.Models.Artist> Artist { get; set; }

        public DbSet<MusicReviewsWebsite.Models.Album> Album { get; set; }
    }
}
