using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicReviewsWebsite.Migrations
{
    public partial class ReviewDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GenreSuggestion_Album_AlbumId",
                table: "GenreSuggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_GenreSuggestion_AspNetUsers_ApplicationUserId",
                table: "GenreSuggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_GenreSuggestion_Genre_GenreId",
                table: "GenreSuggestion");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Review",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "GenreId",
                table: "GenreSuggestion",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "GenreSuggestion",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AlbumId",
                table: "GenreSuggestion",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GenreSuggestion_Album_AlbumId",
                table: "GenreSuggestion",
                column: "AlbumId",
                principalTable: "Album",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GenreSuggestion_AspNetUsers_ApplicationUserId",
                table: "GenreSuggestion",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GenreSuggestion_Genre_GenreId",
                table: "GenreSuggestion",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GenreSuggestion_Album_AlbumId",
                table: "GenreSuggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_GenreSuggestion_AspNetUsers_ApplicationUserId",
                table: "GenreSuggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_GenreSuggestion_Genre_GenreId",
                table: "GenreSuggestion");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Review");

            migrationBuilder.AlterColumn<int>(
                name: "GenreId",
                table: "GenreSuggestion",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "GenreSuggestion",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "AlbumId",
                table: "GenreSuggestion",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_GenreSuggestion_Album_AlbumId",
                table: "GenreSuggestion",
                column: "AlbumId",
                principalTable: "Album",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GenreSuggestion_AspNetUsers_ApplicationUserId",
                table: "GenreSuggestion",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GenreSuggestion_Genre_GenreId",
                table: "GenreSuggestion",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id");
        }
    }
}
