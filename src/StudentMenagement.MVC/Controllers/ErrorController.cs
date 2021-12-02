using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement.Controllers
{
    public class ErrorController:Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this._logger = logger;
        }
        
        [Route("Error/{statusCode}")]
        [HttpGet]
        public IActionResult HttpStatusCodeHandler(int statusCode) 
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
           
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "抱歉，你访问的页面不存在";

                    _logger.LogWarning($"发生了一个404错误，路径={statusCodeResult.OriginalPath}以及查询字符串={statusCodeResult.OriginalQueryString}");

                    //ViewBag.OriginalPath = statusCodeResult.OriginalPath;
                    //ViewBag.OriginalQueryString = statusCodeResult.OriginalQueryString;

                    break;
            }
            return View("NotFound");
        }

        [AllowAnonymous]
        [Route("Error")]
        [HttpGet]
        public IActionResult Error() 
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            _logger.LogError($"路径：{exceptionHandlerPathFeature},产生了一个错误信息{exceptionHandlerPathFeature.Error}");

            //ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
            //ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            //ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;

            return View("Error");
        }
    }
}
