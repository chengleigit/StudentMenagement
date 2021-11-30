using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentMenagement.Models;

namespace StudentMenagement.Infrastructure.EntityMapper
{
    public class StudentCourseMapper:IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {
            //修改表名为Enrollment，设置StudentCourseId为主键Id
            builder.ToTable("Enrollment").HasKey(a => a.StudentCourseId);

            //StudentCourses关联实体Student，设置外键ID为StudentID
            builder.HasOne(a => a.Student).WithMany(a => a.StudentCourses).HasForeignKey(a => a.StudentID);
            builder.HasOne(a => a.Course).WithMany(a => a.StudentCourses).HasForeignKey(a => a.CourseID);
        }
    }
}
