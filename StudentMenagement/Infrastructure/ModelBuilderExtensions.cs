using Microsoft.EntityFrameworkCore;
using StudentMenagement.Models;
using StudentMenagement.Models.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement.Infrastructure
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Student>().HasData(
               new Student()
               {
                   Id = 1,
                   Name = "张三",
                   MaJob = MaEnum.FirstGrade,
                   Email = "@ww.com"
               });
            modelBuilder.Entity<Student>().HasData(
                new Student()
                {
                    Id = 2,
                    Name = "历史",
                    MaJob = MaEnum.FirstGrade,
                    Email = "@lisi.com"
                });
            modelBuilder.Entity<Student>().HasData(
                new Student()
                {
                    Id = 3,
                    Name = "赵六",
                    MaJob = MaEnum.FirstGrade,
                    Email = "@zhaoliu.com"
                });
        }
    }
}
