using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentMenagement.Migrations
{
    public partial class altherStuden : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Studnets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Studnets",
                columns: new[] { "Id", "Email", "MaJob", "Name", "PhotoPath" },
                values: new object[] { 3, "@zhaoliu.com", 1, "赵六", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Studnets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Studnets");
        }
    }
}
