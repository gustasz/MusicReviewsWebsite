using Microsoft.AspNetCore.Identity;
using MusicReviewsWebsite.Models;
using System;
using System.Linq;

namespace MusicReviewsWebsite.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(MusicContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            context.Database.EnsureCreated();

            // Look for any artists or roles
            if (context.Artist.Any() && roleManager.Roles.Any())
            {
                return;   // DB has been seeded
            }

            if (!context.Artist.Any())
            {
                List<Artist> artists = new();
                List<Album> albums = new();

                var artist = new Artist { Name = "Playboi Carti", AlsoKnownAs = "Jordan Terrell Carter [birth name]", BirthDate = DateTime.Parse("1995-09-13") };
                var album = new Album { Name = "Whole Lotta Red", ReleaseDate = DateTime.Parse("2020-12-25"), CoverPath = "Images\\Album Covers\\u024fgcq.f0y.png" };
                artist.Albums.Add(album);
                artists.Add(artist);
                albums.Add(album);

                artist = new Artist { Name = "Porter Robinson", AlsoKnownAs = "Porter Weston Robinson [birth name]", BirthDate = DateTime.Parse("1992-07-15") };
                album = new Album { Name = "Worlds", ReleaseDate = DateTime.Parse("2014-08-12"), CoverPath = "Images\\Album Covers\\mcconegl.0xh.jpg" };
                artist.Albums.Add(album);
                artists.Add(artist);
                albums.Add(album);

                artist = new Artist { Name = "Tyler, the Creator", AlsoKnownAs = "Tyler Gregory Okonma [birth name]", BirthDate = DateTime.Parse("1991-03-06") };
                album = new Album { Name = "IGOR", ReleaseDate = DateTime.Parse("2019-05-17"), CoverPath = "Images\\Album Covers\\acnzpyh0.dlb.jpg" };
                artist.Albums.Add(album);
                artists.Add(artist);
                albums.Add(album);

                artist = new Artist { Name = "Kendrick Lamar", AlsoKnownAs = "Kendrick Lamar Duckworth [birth name]", BirthDate = DateTime.Parse("1987-06-17") };
                album = new Album { Name = "good kid, m.A.A.d city", ReleaseDate = DateTime.Parse("2012-10-22"), CoverPath = "Images\\Album Covers\\bvsy5oz5.f41.jpg" };
                artist.Albums.Add(album);
                artists.Add(artist);
                albums.Add(album);

                artist = new Artist { Name = "Kanye West", AlsoKnownAs = "Kanye Omari West [birth name]", BirthDate = DateTime.Parse("1977-06-08") };
                album = new Album { Name = "My Beautiful Dark Twisted Fantasy", ReleaseDate = DateTime.Parse("2010-11-22"), CoverPath = "Images\\Album Covers\\vcokzuut.pdh.jpg" };
                artist.Albums.Add(album);
                artists.Add(artist);
                albums.Add(album);

                context.Artist.AddRange(artists);
                context.Album.AddRange(albums);
                context.SaveChanges();
            }

            //initialize users and roles
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Moderator"));
            }

            if (!context.Users.Any()) // testing only, change password
            {
                var adminUser = new ApplicationUser
                {
                    Name = "TheAdmin",
                    UserName = "admin@gmail.com"
                };
                await userManager.CreateAsync(adminUser, "Admin1!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
