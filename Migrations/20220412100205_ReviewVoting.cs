using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicReviewsWebsite.Migrations
{
    public partial class ReviewVoting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReviewVote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewId = table.Column<int>(type: "int", nullable: true),
                    IsFor = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewVote_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReviewVote_Review_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Review",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewVote_ApplicationUserId",
                table: "ReviewVote",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewVote_ReviewId",
                table: "ReviewVote",
                column: "ReviewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewVote");
        }
    }
}
