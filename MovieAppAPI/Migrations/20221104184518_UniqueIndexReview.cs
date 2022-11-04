using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAppAPI.Migrations
{
    public partial class UniqueIndexReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_review_MovieId",
                table: "review");

            migrationBuilder.CreateIndex(
                name: "IX_review_MovieId_Author",
                table: "review",
                columns: new[] { "MovieId", "Author" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_review_MovieId_Author",
                table: "review");

            migrationBuilder.CreateIndex(
                name: "IX_review_MovieId",
                table: "review",
                column: "MovieId");
        }
    }
}
