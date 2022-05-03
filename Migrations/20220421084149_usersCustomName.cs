using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicReviewsWebsite.Migrations
{
    public partial class usersCustomName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewVote_AspNetUsers_ApplicationUserId",
                table: "ReviewVote");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewVote_Review_ReviewId",
                table: "ReviewVote");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewId",
                table: "ReviewVote",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "ReviewVote",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewVote_AspNetUsers_ApplicationUserId",
                table: "ReviewVote",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewVote_Review_ReviewId",
                table: "ReviewVote",
                column: "ReviewId",
                principalTable: "Review",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewVote_AspNetUsers_ApplicationUserId",
                table: "ReviewVote");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewVote_Review_ReviewId",
                table: "ReviewVote");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewId",
                table: "ReviewVote",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "ReviewVote",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewVote_AspNetUsers_ApplicationUserId",
                table: "ReviewVote",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewVote_Review_ReviewId",
                table: "ReviewVote",
                column: "ReviewId",
                principalTable: "Review",
                principalColumn: "Id");
        }
    }
}
