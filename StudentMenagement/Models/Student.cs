﻿using StudentMenagement.Models.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement.Models
{
    /// <summary>
    /// 学生类
    /// </summary>
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public MaEnum? MaJor { get; set; }
     
        public string Email { get; set; }

        public string PhotoPath { get; set; }
    }
}
