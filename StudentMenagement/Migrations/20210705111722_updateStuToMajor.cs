using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentMenagement.Migrations
{
    public partial class updateStuToMajor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaJob",
                table: "Studnets",
                newName: "MaJor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaJor",
                table: "Studnets",
                newName: "MaJob");
        }
    }
}
