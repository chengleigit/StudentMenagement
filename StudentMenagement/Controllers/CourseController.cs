using Microsoft.AspNetCore.Mvc;
using StudentMenagement.DataRepositories;

namespace StudentMenagement.Controllers
{
    public class CourseController:Controller
    {
        private readonly ICourseRepository _courseRepository;
        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        // 不填写 [HttpGet]默认为处理GET请求
        public ActionResult Index()
        {
            return View();
        }
    }
}
