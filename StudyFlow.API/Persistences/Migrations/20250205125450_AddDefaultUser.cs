using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudyFlow.API.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1AC74F3B-4C92-42B7-9E6E-AD7E75E5771C", "15F497FA-8AC2-4F62-987C-1BB31EA5F9A3", false, false, "Admin", "ADMIN" },
                    { "3C5D8A1F-7B44-4E99-AC61-5F7D3B29E4A8", "C8A1F3D5-6B29-4F77-91E2-4D9B5C6A731F", false, false, "Guest", "GUEST" },
                    { "8F12A3D5-6C44-4F99-AE35-2D9B4C5E8A71", "A71C4D8F-3B29-4F61-82E7-9D4A6C8F371B", false, false, "Parent", "PARENT" },
                    { "D2A3C1E5-8B22-4C78-9F44-3B8B5F4A7E6D", "5F9A3D7B-6C12-4F88-BE47-1A2F9C8D6712", false, false, "Teacher", "TEACHER" },
                    { "E99790C2-ED3B-4C39-9C74-9C7513547029", "ADDFBE9A-C49B-48E2-8F75-900DB06F5777", true, false, "Student", "STUDENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1AC74F3B-4C92-42B7-9E6E-AD7E75E5771C");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3C5D8A1F-7B44-4E99-AC61-5F7D3B29E4A8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8F12A3D5-6C44-4F99-AE35-2D9B4C5E8A71");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "D2A3C1E5-8B22-4C78-9F44-3B8B5F4A7E6D");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "E99790C2-ED3B-4C39-9C74-9C7513547029");
        }
    }
}
