using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentMenagement.Models;
using System.Linq;

namespace StudentMenagement.Infrastructure
{
    public class AppDbContext :IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Studnets { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        /// <summary>
        /// 播种数据
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //获取当前系统所有领域模型的外键列表
            var foreignKeys = modelBuilder.Model.GetEntityTypes().SelectMany(e=>e.GetForeignKeys());

            foreach (var foreignKey in foreignKeys)
            {
                //将它们的删除行为配置为Restrict，即无操作
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Seed();

            ///指定实体在数据库中生成的名称
            modelBuilder.Entity<Course>().ToTable("Course","School");
            modelBuilder.Entity<StudentCourse>().ToTable("StudentCourse");
            modelBuilder.Entity<Student>().ToTable("Student");
        }
    }
}