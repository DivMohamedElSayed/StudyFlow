using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyFlow.API.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class addExternalSignIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalProviderId",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EC453D56-FD0A-467B-9250-AD00F5CFEAF6",
                columns: new[] { "ExternalProviderId", "Provider" },
                values: new object[] { null, "local" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalProviderId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "AspNetUsers");
        }
    }
}
