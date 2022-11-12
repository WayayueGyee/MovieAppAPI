using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAppAPI.Migrations
{
    public partial class MovieGenreManyToManyFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_genre_movie_MovieId",
                table: "genre");

            migrationBuilder.DropIndex(
                name: "IX_genre_MovieId",
                table: "genre");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "genre");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MovieId",
                table: "genre",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_genre_MovieId",
                table: "genre",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_genre_movie_MovieId",
                table: "genre",
                column: "MovieId",
                principalTable: "movie",
                principalColumn: "Id");
        }
    }
}
