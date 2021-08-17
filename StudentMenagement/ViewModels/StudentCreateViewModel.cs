using Microsoft.AspNetCore.Http;
using StudentMenagement.Models.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement.ViewModels
{
    public class StudentCreateViewModel
    {

        [Display(Name = "名字")]
        [Required(ErrorMessage = "请输入名字，它不能为空！")]
        public string Name { get; set; }
        [Display(Name = "主修科目")]
        [Required(ErrorMessage = "请选择主修科目")]
        public MaEnum? MaJor { get; set; }
        [Display(Name = "电子邮箱")]
        [Required(ErrorMessage = "请输入邮箱地址，它不能为空！")]
        [RegularExpression(@"([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)", ErrorMessage = "邮箱格式错误")]
        public string Email { get; set; }
        [Display(Name = "头像")]
        public List<IFormFile> Photos { get; set; }
    }


}
