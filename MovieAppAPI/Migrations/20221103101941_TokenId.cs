using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MovieAppAPI.Migrations
{
    public partial class TokenId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_valid_token",
                table: "valid_token");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "valid_token",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_valid_token",
                table: "valid_token",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_valid_token",
                table: "valid_token");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "valid_token");

            migrationBuilder.AddPrimaryKey(
                name: "PK_valid_token",
                table: "valid_token",
                column: "Token");
        }
    }
}
