using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentMenagement.Migrations
{
    public partial class AltherStudentSeddData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Studnets",
                columns: new[] { "Id", "Email", "MaJob", "Name" },
                values: new object[] { 2, "@lisi.com", 1, "历史" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Studnets",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
