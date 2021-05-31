using Microsoft.AspNetCore.Mvc;
using StudentMenagement.DataRepositories;
using StudentMenagement.Models;
using StudentMenagement.ViewModels;

namespace StudentMenagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studnetRepository;

        public HomeController(IStudentRepository studnetRepository)
        {
            _studnetRepository = studnetRepository;
        }

        public ActionResult Index()
        {
            var model = _studnetRepository.GetAllStudents();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                Student student_new = _studnetRepository.Insert(student);
                //return RedirectToAction("Details", new { id = student_new.Id });
            }
            return View();
        }

        public ViewResult Details(int? Id)
        {
            Student model = _studnetRepository.GetStudent(Id ?? 1);
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Student = model,
                Title = "学生详情"
            };
            return View(homeDetailsViewModel);
        }

        public string Detele(int Id)
        {
            var delStu = _studnetRepository.Delete(Id);
            return "删除成功";
        }

        //public string Details(int? Id,string name)
        //{
        //    return string.Format("ID编号：{0}，姓名：{1}",Id,name);
        //}
    }
}