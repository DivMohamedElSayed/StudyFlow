using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyFlow.API.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class AddThemeConstantsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThemePreference",
                table: "AspNetUsers",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "default",
                comment: "User's theme preference (dark/light/default)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThemePreference",
                table: "AspNetUsers");
        }
    }
}
