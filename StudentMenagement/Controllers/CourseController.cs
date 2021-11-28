using Microsoft.AspNetCore.Mvc;
using StudentMenagement.Application.Courses;
using StudentMenagement.Application.Dtos;
using StudentMenagement.DataRepositories;
using System.Threading.Tasks;

namespace StudentMenagement.Controllers
{
    public class CourseController:Controller
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // 不填写 [HttpGet]默认为处理GET请求
        public async Task<ActionResult> Index(GetCourseInput input)
        {
            var models = await _courseService.GetPaginatedResult(input);
            return View(models);
        }
    }
}
