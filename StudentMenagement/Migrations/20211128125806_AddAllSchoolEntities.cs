using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentMenagement.Migrations
{
    public partial class AddAllSchoolEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentsCourseId",
                schema: "School",
                table: "StudentCourse",
                newName: "StudentCourseId");

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                schema: "School",
                table: "StudentCourse",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentID",
                schema: "School",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseAssignments",
                columns: table => new
                {
                    TeacherID = table.Column<int>(type: "int", nullable: false),
                    CourseID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAssignments", x => new { x.CourseID, x.TeacherID });
                    table.ForeignKey(
                        name: "FK_CourseAssignments_Course_CourseID",
                        column: x => x.CourseID,
                        principalSchema: "School",
                        principalTable: "Course",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseAssignments_Teachers_TeacherID",
                        column: x => x.TeacherID,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Budget = table.Column<decimal>(type: "money", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TeacherID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentID);
                    table.ForeignKey(
                        name: "FK_Departments_Teachers_TeacherID",
                        column: x => x.TeacherID,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OfficeLocations",
                columns: table => new
                {
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeLocations", x => x.TeacherId);
                    table.ForeignKey(
                        name: "FK_OfficeLocations_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_DepartmentID",
                schema: "School",
                table: "Course",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssignments_TeacherID",
                table: "CourseAssignments",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_TeacherID",
                table: "Departments",
                column: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Departments_DepartmentID",
                schema: "School",
                table: "Course",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Departments_DepartmentID",
                schema: "School",
                table: "Course");

            migrationBuilder.DropTable(
                name: "CourseAssignments");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "OfficeLocations");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Course_DepartmentID",
                schema: "School",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Grade",
                schema: "School",
                table: "StudentCourse");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                schema: "School",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "StudentCourseId",
                schema: "School",
                table: "StudentCourse",
                newName: "StudentsCourseId");
        }
    }
}
