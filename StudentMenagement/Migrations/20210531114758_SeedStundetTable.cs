using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentMenagement.Migrations
{
    public partial class SeedStundetTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Studnets",
                columns: new[] { "Id", "Email", "MaJob", "Name" },
                values: new object[] { 1, "@ww.com", 1, "张三" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Studnets",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
