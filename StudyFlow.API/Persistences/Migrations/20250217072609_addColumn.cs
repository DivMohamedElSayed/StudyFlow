using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyFlow.API.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class addColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PreferredSubject",
                table: "Students",
                newName: "PreferredSubjects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PreferredSubjects",
                table: "Students",
                newName: "PreferredSubject");
        }
    }
}
