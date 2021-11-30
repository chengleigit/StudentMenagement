using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentMenagement.Migrations
{
    public partial class AddStudentCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Course_CourseID",
                schema: "School",
                table: "StudentCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Person_StudentID",
                schema: "School",
                table: "StudentCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentCourse",
                schema: "School",
                table: "StudentCourse");

            migrationBuilder.RenameTable(
                name: "StudentCourse",
                schema: "School",
                newName: "Enrollment");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourse_StudentID",
                table: "Enrollment",
                newName: "IX_Enrollment_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourse_CourseID",
                table: "Enrollment",
                newName: "IX_Enrollment_CourseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrollment",
                table: "Enrollment",
                column: "StudentCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Course_CourseID",
                table: "Enrollment",
                column: "CourseID",
                principalSchema: "School",
                principalTable: "Course",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Person_StudentID",
                table: "Enrollment",
                column: "StudentID",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Course_CourseID",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Person_StudentID",
                table: "Enrollment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrollment",
                table: "Enrollment");

            migrationBuilder.RenameTable(
                name: "Enrollment",
                newName: "StudentCourse",
                newSchema: "School");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_StudentID",
                schema: "School",
                table: "StudentCourse",
                newName: "IX_StudentCourse_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_CourseID",
                schema: "School",
                table: "StudentCourse",
                newName: "IX_StudentCourse_CourseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentCourse",
                schema: "School",
                table: "StudentCourse",
                column: "StudentCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourse_Course_CourseID",
                schema: "School",
                table: "StudentCourse",
                column: "CourseID",
                principalSchema: "School",
                principalTable: "Course",
                principalColumn: "CourseID",
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
    }
}
