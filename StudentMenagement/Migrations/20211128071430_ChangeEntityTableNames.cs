﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentMenagement.Migrations
{
    public partial class ChangeEntityTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_Courses_CourseID",
                table: "StudentCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_Studnets_StudentID",
                table: "StudentCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Studnets",
                table: "Studnets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentCourses",
                table: "StudentCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.EnsureSchema(
                name: "School");

            migrationBuilder.RenameTable(
                name: "Studnets",
                newName: "Student");

            migrationBuilder.RenameTable(
                name: "StudentCourses",
                newName: "StudentCourse");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Course",
                newSchema: "School");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourses_StudentID",
                table: "StudentCourse",
                newName: "IX_StudentCourse_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourses_CourseID",
                table: "StudentCourse",
                newName: "IX_StudentCourse_CourseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student",
                table: "Student",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentCourse",
                table: "StudentCourse",
                column: "StudentsCourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                schema: "School",
                table: "Course",
                column: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourse_Course_CourseID",
                table: "StudentCourse",
                column: "CourseID",
                principalSchema: "School",
                principalTable: "Course",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourse_Student_StudentID",
                table: "StudentCourse",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Course_CourseID",
                table: "StudentCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Student_StudentID",
                table: "StudentCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentCourse",
                table: "StudentCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Student",
                table: "Student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                schema: "School",
                table: "Course");

            migrationBuilder.RenameTable(
                name: "StudentCourse",
                newName: "StudentCourses");

            migrationBuilder.RenameTable(
                name: "Student",
                newName: "Studnets");

            migrationBuilder.RenameTable(
                name: "Course",
                schema: "School",
                newName: "Courses");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourse_StudentID",
                table: "StudentCourses",
                newName: "IX_StudentCourses_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourse_CourseID",
                table: "StudentCourses",
                newName: "IX_StudentCourses_CourseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentCourses",
                table: "StudentCourses",
                column: "StudentsCourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Studnets",
                table: "Studnets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_Courses_CourseID",
                table: "StudentCourses",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_Studnets_StudentID",
                table: "StudentCourses",
                column: "StudentID",
                principalTable: "Studnets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
