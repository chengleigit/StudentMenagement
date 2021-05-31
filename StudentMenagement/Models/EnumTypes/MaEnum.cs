using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement.Models.EnumTypes
{
    public enum MaEnum
    {
        [Display(Name="未分配")]
        None,
        [Display(Name = "计算机科学")]
        FirstGrade,
        [Display(Name = "软件技术")]
        SecondGrade,
        [Display(Name = "数学")]
        GradeThree
    }
}
