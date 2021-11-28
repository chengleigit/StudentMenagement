using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentMenagement.Migrations
{
    public partial class removeSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "School",
                table: "Student",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "School",
                table: "Student",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "School",
                table: "Student",
                keyColumn: "Id",
                keyValue: 3);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "School",
                table: "Student",
                columns: new[] { "Id", "Email", "EnrollmentDate", "MaJor", "Name", "PhotoPath" },
                values: new object[] { 1, "@ww.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "张三", null });

            migrationBuilder.InsertData(
                schema: "School",
                table: "Student",
                columns: new[] { "Id", "Email", "EnrollmentDate", "MaJor", "Name", "PhotoPath" },
                values: new object[] { 2, "@lisi.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "历史", null });

            migrationBuilder.InsertData(
                schema: "School",
                table: "Student",
                columns: new[] { "Id", "Email", "EnrollmentDate", "MaJor", "Name", "PhotoPath" },
                values: new object[] { 3, "@zhaoliu.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "赵六", null });
        }
    }
}
