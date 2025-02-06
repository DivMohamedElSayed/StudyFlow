using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyFlow.API.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccessToken",
                table: "RefreshTokens",
                newName: "RefreshTokenCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshTokenCode",
                table: "RefreshTokens",
                newName: "AccessToken");
        }
    }
}
