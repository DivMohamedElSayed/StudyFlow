using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyFlow.API.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class AlterSubjectTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferredSubjects",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "PreferredSubject",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferredSubject",
                table: "Students");

            migrationBuilder.AddColumn<string[]>(
                name: "PreferredSubjects",
                table: "Students",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }
    }
}
