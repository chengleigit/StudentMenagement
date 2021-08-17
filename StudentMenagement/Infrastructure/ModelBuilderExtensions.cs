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
            modelBuilder.Entity<Student>().HasData(
               new Student()
               {
                   Id = 1,
                   Name = "张三",
                   MaJor = MaEnum.ComputerScience,
                   Email = "@ww.com"
               });
            modelBuilder.Entity<Student>().HasData(
                new Student()
                {
                    Id = 2,
                    Name = "历史",
                    MaJor = MaEnum.Mathematics,
                    Email = "@lisi.com"
                });
            modelBuilder.Entity<Student>().HasData(
                new Student()
                {
                    Id = 3,
                    Name = "赵六",
                    MaJor = MaEnum.ElectronicCommerce,
                    Email = "@zhaoliu.com"
                });
        }
    }
}
