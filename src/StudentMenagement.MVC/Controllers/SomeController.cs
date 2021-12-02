using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement.Controllers
{
    [Authorize(Roles="admin,User")]
    public class SomeController: Controller
    {
        public string ABC() 
        {
            return "我是方法ABC,只要拥有Admin或者User角色即可访问我";
        }

        [Authorize(Roles ="admin")]
        public string XYZ()
        {
            return "我是方法XYZ,只要拥有Admin角色即可访问我";
        }

        [AllowAnonymous]
        public string Anyone()
        {
            return "任何人都可以访问Anyone,因为我添加的AllowAnonymous属性";
        }
    }
}
