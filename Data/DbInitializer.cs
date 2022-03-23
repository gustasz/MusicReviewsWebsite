using MusicReviewsWebsite.Models;
using System;
using System.Linq;

namespace MusicReviewsWebsite.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MusicContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Artist.Any())
            {
                return;   // DB has been seeded
            }

            var artists = new Artist[]
            {
            new Artist{Name="Playboi Carti", AlsoKnownAs="Jordan Terrell Carter [birth name]",BirthDate=DateTime.Parse("1995-09-13")},
            new Artist{Name="Porter Robinson", AlsoKnownAs="Porter Weston Robinson [birth name]",BirthDate=DateTime.Parse("1992-07-15")},
            new Artist{Name="Tyler, the Creator", AlsoKnownAs="Tyler Gregory Okonma [birth name]",BirthDate=DateTime.Parse("1991-03-06")},
            new Artist{Name="Kendrick Lamar", AlsoKnownAs="Kendrick Lamar Duckworth [birth name]",BirthDate=DateTime.Parse("1987-06-17")},
            new Artist{Name="Kanye West", AlsoKnownAs="Kanye Omari West [birth name]",BirthDate=DateTime.Parse("1977-06-08")}
            };

            context.Artist.AddRange(artists);
            context.SaveChanges();

        /*    var albums = new Album[]
            {
            new Album{Name="Whole Lotta Red",ReleaseDate=DateTime.Parse("2020-12-25"),ArtistId = 1},
            new Album{Name="Worlds",ReleaseDate=DateTime.Parse("2014-08-12"),ArtistId = 2},
            new Album{Name="IGOR",ReleaseDate=DateTime.Parse("2019-05-17"),ArtistId = 3},
            new Album{Name="good kid, m.A.A.d city",ReleaseDate=DateTime.Parse("2012-10-22"),ArtistId = 4},
            new Album{Name="My Beautiful Dark Twisted Fantasy",ReleaseDate=DateTime.Parse("2010-11-22"),ArtistId = 5},
            };

            context.Album.AddRange(albums);
            context.SaveChanges();*/

        }
    }
}
