using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentMenagement.DataRepositories;
using StudentMenagement.Models;
using StudentMenagement.ViewModels;
using System;
using System.IO;

namespace StudentMenagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studnetRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IStudentRepository studnetRepository,IWebHostEnvironment webHostEnvironment,ILogger<HomeController> logger)
        {
            _studnetRepository = studnetRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
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
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                //单个图片上传
                //if (model.Photos!=null)
                //{
                //    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                //    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photos.FileName;
                //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //    model.Photos.CopyTo(new FileStream(filePath, FileMode.Create));
                //}

                foreach (IFormFile photo in model.Photos)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images","avatars");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Student newStuent = new Student()
                {
                    Name = model.Name,
                    Email = model.Email,
                    MaJor = model.MaJor,
                    PhotoPath = uniqueFileName
                };
                _studnetRepository.Insert(newStuent);

                return RedirectToAction("Details", new { id=newStuent.Id });

            }
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int Id) 
        {
            Student stu = _studnetRepository.GetStudent(Id);

            StudentEditViewModel studentEditViewModel = new StudentEditViewModel()
            {
                Id = stu.Id,
                Name = stu.Name,
                Email = stu.Email,
                MaJor = stu.MaJor,
                ExistingPhotoPath = stu.PhotoPath
            };

            return View(studentEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(StudentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Student stu = _studnetRepository.GetStudent(model.Id);

                stu.Name = model.Name;
                stu.Email = model.Email;
                stu.MaJor = model.MaJor;

                //如果修改图片
                if (model.Photos!=null&& model.Photos.Count>0)
                {
                    if (model.ExistingPhotoPath!=null)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "avatars",model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    stu.PhotoPath= ProcessUploadFile(model);
                }

                Student updatedstudnet = _studnetRepository.Update(stu);
      
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ViewResult Details(int Id)
        {
            //throw new Exception("在Details视图中抛出异常！！");

            _logger.LogTrace("Trace（跟踪）log");
            _logger.LogDebug("Debug（调试）log");
            _logger.LogInformation("Information（信息）log");
            _logger.LogWarning("Warning（警告）log");
            _logger.LogError("Error（错误）log");
            _logger.LogCritical("Critical（严重）log");




            Student student = _studnetRepository.GetStudent(Id);

            if (student==null)
            {
                Response.StatusCode = 404;

                return View("StudnetNoFound",Id);
            }
    
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Student = student,
                Title = "学生详情"
            };
            return View(homeDetailsViewModel);
        }

        public string Detele(int Id)
        {
            var delStu = _studnetRepository.Delete(Id);
            return "删除成功";
        }

        /// <summary>
        /// 将文件保存到指定目录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ProcessUploadFile(StudentCreateViewModel model) 
        {
            string uniqueFileName = null;
            foreach (IFormFile photo in model.Photos)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "avatars");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream=new FileStream(filePath,FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }
                
            }
            return uniqueFileName;
        }

        //public string Details(int? Id,string name)
        //{
        //    return string.Format("ID编号：{0}，姓名：{1}",Id,name);
        //}
    }
}