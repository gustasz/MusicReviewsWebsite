using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicReviewsWebsite.Migrations
{
    public partial class ManyToManyAlbumArtists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_Artist_ArtistId",
                table: "Album");

            migrationBuilder.DropIndex(
                name: "IX_Album_ArtistId",
                table: "Album");

            migrationBuilder.DropColumn(
                name: "ArtistId",
                table: "Album");

            migrationBuilder.CreateTable(
                name: "AlbumArtist",
                columns: table => new
                {
                    AlbumsId = table.Column<int>(type: "int", nullable: false),
                    ArtistsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumArtist", x => new { x.AlbumsId, x.ArtistsId });
                    table.ForeignKey(
                        name: "FK_AlbumArtist_Album_AlbumsId",
                        column: x => x.AlbumsId,
                        principalTable: "Album",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumArtist_Artist_ArtistsId",
                        column: x => x.ArtistsId,
                        principalTable: "Artist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumArtist_ArtistsId",
                table: "AlbumArtist",
                column: "ArtistsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumArtist");

            migrationBuilder.AddColumn<int>(
                name: "ArtistId",
                table: "Album",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Album_ArtistId",
                table: "Album",
                column: "ArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Album_Artist_ArtistId",
                table: "Album",
                column: "ArtistId",
                principalTable: "Artist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
