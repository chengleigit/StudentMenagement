﻿using System;
using System.ComponentModel.DataAnnotations;

namespace StudentMenagement.Application.Dtos
{
    /// <summary>
    /// 入学时间分组
    /// </summary>
    public class EnrollmentDateGroupDto
    {

        /// <summary>
        /// 入学时间
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
        public DateTime? EnrollmentDate { get; set; }

        public int StudentCount { get; set; }
    }
}
