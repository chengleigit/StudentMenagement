using Microsoft.EntityFrameworkCore;
using StudentMenagement.Models;
using StudentMenagement.Models.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement.Infrastructure
{
    /// <summary>
    /// ModelBuilder扩展类
    /// </summary>
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder) 
        {
            //modelBuilder.Entity<Student>().HasData(
            //   new Student()
            //   {
            //       Id = 1,
            //       Name = "张三",
            //       MaJor = MaEnum.ComputerScience,
            //       Email = "@ww.com"
            //   });
            //modelBuilder.Entity<Student>().HasData(
            //    new Student()
            //    {
            //        Id = 2,
            //        Name = "历史",
            //        MaJor = MaEnum.Mathematics,
            //        Email = "@lisi.com"
            //    });
            //modelBuilder.Entity<Student>().HasData(
            //    new Student()
            //    {
            //        Id = 3,
            //        Name = "赵六",
            //        MaJor = MaEnum.ElectronicCommerce,
            //        Email = "@zhaoliu.com"
            //    });

            ///指定实体在数据库中生成的名称
            modelBuilder.Entity<Course>().ToTable("Course", "School");
            modelBuilder.Entity<StudentCourse>().ToTable("StudentCourse", "School");
            //modelBuilder.Entity<Student>().ToTable("Student", "School");
            modelBuilder.Entity<Person>().ToTable("Person");

            modelBuilder.Entity<CourseAssignment>().HasKey(c => new { c.CourseID,c.TeacherID });


            //modelBuilder.Entity<Blog>().ToTable("Blogs").HasKey(a => a.Id);
            //modelBuilder.Entity<Blog>().Property(a => a.Title).HasMaxLength(50).HasColumnName("BlogTitle");

        }

        
    }
}
