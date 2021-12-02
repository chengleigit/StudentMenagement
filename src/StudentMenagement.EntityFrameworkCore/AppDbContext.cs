
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentMenagement.Infrastructure.EntityMapper;
using StudentMenagement.Models;
using System.Linq;

namespace StudentMenagement.Infrastructure
{
    public class AppDbContext :IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<OfficeLocation> OfficeLocations { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Blog> Blogs { get;set; }
        public DbSet<BlogImage> BlogImages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }


        /// <summary>
        /// 播种数据
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();

            //获取当前系统所有领域模型的外键列表
            var foreignKeys = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());

            foreach (var foreignKey in foreignKeys)
            {
                //将它们的删除行为配置为Restrict,即无操作
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //启用配置
            modelBuilder.ApplyConfiguration(new BlogMapper());
            modelBuilder.ApplyConfiguration(new PostMapper());
            modelBuilder.ApplyConfiguration(new StudentCourseMapper());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies()
            //.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection"));
            
        }
    }
}