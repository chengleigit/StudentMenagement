using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using StudentMenagement.Models;
using StudentMenagement.Models.EnumTypes;
using System;
using System.Linq;

namespace StudentMenagement.Infrastructure.Data
{
    public static class DataInitializer
    {
        public static IApplicationBuilder UseDataInitializer(
        this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var dbcontext = scope.ServiceProvider.GetService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                #region 学生种子信息

                if (dbcontext.Students.Any())
                {
                    return builder;// 数据已经初始化了
                }

                var students = new[]
                {
                new Student
                {
                    Name = "张三",MaJor = MaEnum.ComputerScience,Email = "zhangsan@52abp.com",
                    EnrollmentDate = DateTime.Parse("2016-09-01"),
                },
                new Student
                {
                    Name = "李四",MaJor = MaEnum.Mathematics,Email = "lisi@52abp.com",
                    EnrollmentDate = DateTime.Parse("2017-09-01")
                },
                new Student
                {
                    Name = "王五",MaJor = MaEnum.ElectronicCommerce,Email = "wangwu@52abp.com",
                    EnrollmentDate = DateTime.Parse("2012-09-01")
                }
            };
                foreach (Student item in students)
                {
                    dbcontext.Students.Add(item);
                }

                dbcontext.SaveChanges();

                #endregion 学生种子信息

                #region 学院种子数据

                var teachers = new[]
                {
                new Teacher{Name = "张老师",HireDate = DateTime.Parse ("1995-03-11")},
                new Teacher{Name = "王老师",HireDate = DateTime.Parse ("2003-03-11")},
                new Teacher{Name = "李老师",HireDate = DateTime.Parse ("1990-03-11")},
                new Teacher{Name = "赵老师",HireDate = DateTime.Parse ("1985-03-11")},
                new Teacher{Name = "刘老师",HireDate = DateTime.Parse ("2003-03-11")},
                new Teacher{Name = "胡老师",HireDate = DateTime.Parse ("2003-03-11")}
            };

                foreach (var i in teachers)
                    dbcontext.Teachers.Add(i);
                dbcontext.SaveChanges();

                #endregion 学院种子数据

                var departments = new[]
                {
                new Department
                {
                    Name = "a",Budget = 350000,StartDate = DateTime.Parse("2017-09-01"),
                    TeacherID = teachers.Single(i => i.Name == "刘老师").Id
                },
                new Department
                {
                    Name = "b",Budget = 100000,StartDate = DateTime.Parse("2017-09-01"),
                    TeacherID = teachers.Single(i => i.Name == "赵老师").Id
                },
                new Department
                {
                    Name = "c",Budget = 350000,StartDate = DateTime.Parse("2017-09-01"),
                    TeacherID = teachers.Single(i => i.Name == "胡老师").Id
                },
                new Department
                {
                    Name = "d",Budget = 100000,StartDate = DateTime.Parse("2017-09-01"),
                    TeacherID = teachers.Single(i => i.Name == "王老师").Id
                }
            };

                foreach (var d in departments)
                    dbcontext.Departments.Add(d);
                dbcontext.SaveChanges();

                #region 课程种子数据

                if (dbcontext.Courses.Any())
                {
                    return builder;// 数据已经初始化了
                }

                var courses = new[]
                {
                new Course
                {
                    CourseID = 1050,Title = "数学",Credits = 3,
                    DepartmentID = departments.Single(s => s.Name == "b").DepartmentID
                },
                new Course
                {
                    CourseID = 4022,Title = "政治",Credits = 3,
                    DepartmentID = departments.Single(s => s.Name == "c").DepartmentID
                },
                new Course
                {
                    CourseID = 4041,Title = "物理",Credits = 3,
                    DepartmentID = departments.Single(s => s.Name == "b").DepartmentID
                },
                new Course
                {
                    CourseID = 1045,Title = "化学",Credits = 4,
                    DepartmentID = departments.Single(s => s.Name == "d").DepartmentID
                },
                new Course
                {
                    CourseID = 3141,Title = "生物",Credits = 4,
                    DepartmentID = departments.Single(s => s.Name == "a").DepartmentID
                },
                new Course
                {
                    CourseID = 2021,Title = "英语",Credits = 3,
                    DepartmentID = departments.Single(s => s.Name == "a").DepartmentID
                },
                new Course
                {
                    CourseID = 2042,Title = "历史",Credits = 4,
                    DepartmentID = departments.Single(s => s.Name == "c").DepartmentID
                }
            };

                foreach (var c in courses)
                    dbcontext.Courses.Add(c);
                dbcontext.SaveChanges();

                #endregion 课程种子数据

                #region 办公室分配的种子数据

                var OfficeLocations = new[]
                {
                new OfficeLocation{TeacherId = teachers.Single(i => i.Name == "刘老师").Id,Location = "X楼"},
                new OfficeLocation{TeacherId = teachers.Single(i => i.Name == "胡老师").Id,Location = "Y楼"},
                new OfficeLocation{TeacherId = teachers.Single(i => i.Name == "王老师").Id,Location = "Z楼"}
            };

                foreach (var o in OfficeLocations)
                    dbcontext.OfficeLocations.Add(o);
                dbcontext.SaveChanges();

                #endregion

                #region 为教师分配课程的种子数据

                var coursetTeachers = new[]
                {
                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "数学").CourseID,
                    TeacherID = teachers.Single(i => i.Name == "赵老师").Id
                },
                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "数学").CourseID,
                    TeacherID = teachers.Single(i => i.Name == "王老师").Id
                },
                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "政治").CourseID,
                    TeacherID = teachers.Single(i => i.Name == "胡老师").Id
                },
                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "化学").CourseID,
                    TeacherID = teachers.Single(i => i.Name == "王老师").Id
                },
                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "生物").CourseID,
                    TeacherID = teachers.Single(i => i.Name == "刘老师").Id
                },
                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "英语").CourseID,
                    TeacherID = teachers.Single(i => i.Name == "刘老师").Id
                },
                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "物理").CourseID,
                    TeacherID = teachers.Single(i => i.Name == "赵老师").Id
                },
                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "历史").CourseID,
                    TeacherID = teachers.Single(i => i.Name == "胡老师").Id
                }
            };

                foreach (var ci in coursetTeachers)
                    dbcontext.CourseAssignments.Add(ci);
                dbcontext.SaveChanges();

                #endregion

                #region 学生课程关联种子数据

                var studentCourses = new[]
                {
                new StudentCourse
                {
                    StudentID = students.Single(s => s.Name == "张三").Id,
                    CourseID = courses.Single(c => c.Title == "数学").CourseID,Grade = Grade.A
                },
            };
                foreach (var sc in studentCourses)
                    dbcontext.StudentCourses.Add(sc);
                dbcontext.SaveChanges();

                #endregion 学生课程关联种子数据

                #region 用户种子数据

                if (dbcontext.Users.Any())
                {
                    return builder;// 数据已经初始化了
                }

                var user = new ApplicationUser
                { Email = "10924353902@qq.com", UserName = "1094353902@qq.com", EmailConfirmed = true, City = "合肥" };
                userManager.CreateAsync(user, "1").Wait();// 等待异步方法执行完毕
                dbcontext.SaveChanges();
                var adminRole = "Admin";

                var role = new IdentityRole { Name = adminRole, };

                dbcontext.Roles.Add(role);
                dbcontext.SaveChanges();

                dbcontext.UserRoles.Add(new IdentityUserRole<string>
                {
                    RoleId = role.Id,
                    UserId = user.Id
                });
                dbcontext.SaveChanges();

                #endregion 用户种子数据


                #region 初始化数据弃用

                //      #region 学生种子信息

                //      if (dbcontext.Students.Any())
                //      {
                //          return builder;// 数据已经初始化了
                //      }

                //      var students = new[] {
                //    new Student{Name = "张三",MaJor = MaEnum.ComputerScience,Email = "zhangsan@52abp.com",EnrollmentDate = DateTime.Parse("2016-09-01"),},
                //    new Student{Name = "李四",MaJor = MaEnum.Mathematics,Email = "lisi@52abp.com",EnrollmentDate = DateTime.Parse("2017-09-01") },
                //    new Student{Name = "王五",MaJor = MaEnum.ElectronicCommerce,Email = "wangwu@52abp.com",EnrollmentDate = DateTime.Parse("2012-09-01") }
                //};
                //      foreach (Student item in students)
                //      {
                //          dbcontext.Students.Add(item);
                //      }
                //      dbcontext.SaveChanges();

                //      #endregion 学生种子信息

                //      #region 课程种子数据

                //      if (dbcontext.Courses.Any())
                //      {
                //          return builder;// 数据已经初始化了
                //      }
                //      var courses = new[] {
                //    new Course{CourseID = 1050,Title = "数学",Credits = 3,},
                //    new Course{CourseID = 4022,Title = "政治",Credits = 3,},
                //    new Course{CourseID = 4041,Title = "物理",Credits = 3,},
                //    new Course{CourseID = 1045,Title = "化学",Credits = 4,},
                //    new Course{CourseID = 3141,Title = "生物",Credits = 4,},
                //    new Course{CourseID = 2021,Title = "英语",Credits = 3,},
                //    new Course{CourseID = 2042,Title = "历史",Credits = 4,}
                //};

                //      foreach (var c in courses)
                //          dbcontext.Courses.Add(c);
                //      dbcontext.SaveChanges();

                //      #endregion 课程种子数据

                //      #region 学生课程关联种子数据
                //      //这里学生的ID为4、5、6是因为我们之前的种子数据中已经占了1、2、3的ID
                //      //所以新生成的ID是4开始
                //      var studentCourses = new[] {
                //            new StudentCourse{CourseID = 1050,StudentID = 6,},
                //            new StudentCourse{CourseID = 4022,StudentID = 5,},
                //            new StudentCourse{CourseID = 2021,StudentID = 4,},
                //            new StudentCourse{CourseID = 4022,StudentID = 4,},
                //            new StudentCourse{CourseID = 2021,StudentID = 6,}
                //          };
                //      foreach (var sc in studentCourses)
                //          dbcontext.StudentCourses.Add(sc);
                //      dbcontext.SaveChanges();

                //      #endregion 学生课程关联种子数据

                //      #region 用户种子数据

                //      if (dbcontext.Users.Any())
                //      {
                //          return builder;// 数据已经初始化了
                //      }
                //      var user = new ApplicationUser { Email = "1094353902@qq.com", UserName = "1094353902@qq.com", EmailConfirmed = true, City = "合肥" };
                //      userManager.CreateAsync(user, "1").Wait();// 等待异步方法执行完毕
                //      dbcontext.SaveChanges();
                //      var adminRole = "admin";

                //      var role = new IdentityRole { Name = adminRole, };

                //      dbcontext.Roles.Add(role);
                //      dbcontext.SaveChanges();

                //      dbcontext.UserRoles.Add(new IdentityUserRole<string>
                //      {
                //          RoleId = role.Id,
                //          UserId = user.Id
                //      });
                //      dbcontext.SaveChanges();
                //      #endregion 用户种子数据

                #endregion
            }
            return builder;
        }
    }
}
