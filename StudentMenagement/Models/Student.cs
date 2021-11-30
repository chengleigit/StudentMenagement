using StudentMenagement.Models.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement.Models
{
    /// <summary>
    /// 学生类
    /// </summary>
    public class Student:Person
    {
        //public int Id { get; set; }

        //public string Name { get; set; }

        public MaEnum? MaJor { get; set; }
     
        public string Email { get; set; }

        public string PhotoPath { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }
        /// <summary>
        /// 入学时间
        /// </summary>
        public DateTime EnrollmentDate { get; set; }
        //导航属性
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
