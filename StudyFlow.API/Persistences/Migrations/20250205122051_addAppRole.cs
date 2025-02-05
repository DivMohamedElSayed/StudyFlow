using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyFlow.API.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class addAppRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsDisabled", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "ThemePreference", "TwoFactorEnabled", "UserName" },
                values: new object[] { "EC453D56-FD0A-467B-9250-AD00F5CFEAF6", 0, "3430654E-D391-414E-BE52-35A30D75A969", "admin@study-flow.com", true, "Study", false, "Flow", false, null, "ADMIN@STUDY-FLOW.COM", "ADMIN22STUDYFLOW", "AQAAAAIAAYagAAAAEOuaLGh40PnMyPUT09tk41T2miBgDfaArCPoYUZUfo1hsijP5V5+HXLJ1FBj9vuYSQ==", null, false, "8D12470228E446939E85471A8487C29B", "default", false, "admin22studyflow" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EC453D56-FD0A-467B-9250-AD00F5CFEAF6");
        }
    }
}
