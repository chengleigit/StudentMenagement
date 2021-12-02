using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "邮箱")]
        [Remote(action: "IsEmailInUse", controller: "account")]
        //[ValidEmailDomain(allowedDomain:"1094353902")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]

        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password",
            ErrorMessage = "密码与确认密码不一致，请重新输入.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "所在城市不能为空！")]
        [Display(Name = "城市")]
        public string City { get; set; }
    }
}
