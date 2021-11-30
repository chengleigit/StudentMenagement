using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentMenagement.Migrations
{
    public partial class addPersonEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignments_Teachers_TeacherID",
                table: "CourseAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Teachers_TeacherID",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficeLocations_Teachers_TeacherId",
                table: "OfficeLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Student_StudentID",
                schema: "School",
                table: "StudentCourse");

            migrationBuilder.DropTable(
                name: "Student",
                schema: "School");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers");

            migrationBuilder.RenameTable(
                name: "Teachers",
                newName: "Person");

            migrationBuilder.RenameColumn(
                name: "TeacherName",
                table: "Person",
                newName: "Name");

            migrationBuilder.AlterColumn<DateTime>(
                name: "HireDate",
                table: "Person",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Person",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EnrollmentDate",
                table: "Person",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaJor",
                table: "Person",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Person",
                table: "Person",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignments_Person_TeacherID",
                table: "CourseAssignments",
                column: "TeacherID",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Person_TeacherID",
                table: "Departments",
                column: "TeacherID",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeLocations_Person_TeacherId",
                table: "OfficeLocations",
                column: "TeacherId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourse_Person_StudentID",
                schema: "School",
                table: "StudentCourse",
                column: "StudentID",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignments_Person_TeacherID",
                table: "CourseAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Person_TeacherID",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficeLocations_Person_TeacherId",
                table: "OfficeLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Person_StudentID",
                schema: "School",
                table: "StudentCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Person",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "EnrollmentDate",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "MaJor",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Person");

            migrationBuilder.RenameTable(
                name: "Person",
                newName: "Teachers");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Teachers",
                newName: "TeacherName");

            migrationBuilder.AlterColumn<DateTime>(
                name: "HireDate",
                table: "Teachers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Student",
                schema: "School",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaJor = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignments_Teachers_TeacherID",
                table: "CourseAssignments",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Teachers_TeacherID",
                table: "Departments",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeLocations_Teachers_TeacherId",
                table: "OfficeLocations",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourse_Student_StudentID",
                schema: "School",
                table: "StudentCourse",
                column: "StudentID",
                principalSchema: "School",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
