using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAppAPI.Migrations
{
    public partial class AddedUserRoleAndAvatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "user",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_country_CountryName",
                table: "country",
                column: "CountryName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_country_CountryName",
                table: "country");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "user");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "user");
        }
    }
}
