using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentMenagement.Migrations
{
    public partial class Remove_SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "StudentCourse",
                newName: "StudentCourse",
                newSchema: "School");

            migrationBuilder.RenameTable(
                name: "Student",
                newName: "Student",
                newSchema: "School");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "StudentCourse",
                schema: "School",
                newName: "StudentCourse");

            migrationBuilder.RenameTable(
                name: "Student",
                schema: "School",
                newName: "Student");
        }
    }
}
