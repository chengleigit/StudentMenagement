using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement.Models.EnumTypes
{
    public enum MaEnum
    {
        [Display(Name = "未分配")]
        None,
        [Display(Name = "计算机科学")]
        ComputerScience,
        [Display(Name = "电子商务")]
        ElectronicCommerce,
        [Display(Name = "数学")]
        Mathematics
    }
}
