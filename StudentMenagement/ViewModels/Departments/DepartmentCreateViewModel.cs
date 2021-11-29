﻿using Microsoft.AspNetCore.Mvc.Rendering;
using StudentMenagement.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace StudentMenagement.ViewModels.Departments
{
    public class DepartmentCreateViewModel
    {
        public int DepartmentID { get; set; }

        [StringLength(50,MinimumLength = 3)]
        [Display(Name = "学院名称")]
        public string Name { get; set; }
        /// <summary>
        /// 预算
        /// </summary>
        [DataType(DataType.Currency)]
        [Display(Name = "预算")]
        public decimal Budget { get; set; }

        /// <summary>
        /// 成立时间
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
        [Display(Name = "成立时间")]
        public DateTime StartDate { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Display(Name = "负责人")]
        public SelectList TeacherList { get; set; }

        public int? TeacherID { get; set; }

        public Teacher Administrator { get; set; }
    }
}
