using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentMenagement.DataRepositories;
using StudentMenagement.Infrastructure.Repositories;
using StudentMenagement.Models;
using StudentMenagement.Security.CustomTokenProvider;
using StudentMenagement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace StudentMenagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Student, int> _studentRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;

        //IDataProtector提供了Protect() 和 Unprotect() 方法,可以对数据进行加密或者解密。
        private readonly IDataProtector _protector;

        public HomeController(IRepository<Student, int> studentRepository,
            IWebHostEnvironment webHostEnvironment,
            ILogger<HomeController> logger,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings
            )
        {
            _studentRepository = studentRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _protector = dataProtectionProvider.CreateProtector(
                 dataProtectionPurposeStrings.StudentIdRouteValue);
        }

        public async Task<IActionResult> Index(int? pageNumber,int pageSize = 10,string sortBy = "Id")
        {
            IQueryable<Student> query = _studentRepository.GetAll().OrderBy(sortBy).AsNoTracking();

            //查询所有的学生信息
            List<Student> model = query.ToList().Select(s => {
                //加密ID值并存储在EncryptedId属性中
                s.EncryptedId = _protector.Protect(s.Id.ToString());
                return s;
            }).ToList();

            //将学生列表传递到视图
            return View(model);


            #region 排序
            //ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            //var students = _studentRepository.GetAll();
            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        students = students.OrderByDescending(s => s.Name);
            //        break;
            //    case "Date":
            //        students = students.OrderBy(s => s.EnrollmentDate);
            //        break;
            //    case "date_desc":
            //        students = students.OrderByDescending(s => s.EnrollmentDate);
            //        break;
            //    default:
            //        students = students.OrderBy(s => s.Name);
            //        break;
            //}

            //return View(students);


            #endregion
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
                //封装好的上传图片代码
                var uniqueFileName = ProcessUploadedFile(model);
                Student newStudent = new Student
                {
                    Name = model.Name,
                    Email = model.Email,
                    MaJor = model.MaJor,
                    EnrollmentDate = model.EnrollmentDate,
                    // 将文件名保存在Student对象的PhotoPath属性中
                    //它将保存到数据库Students的表中
                    PhotoPath = uniqueFileName
                };

                _studentRepository.Insert(newStudent);

                var encryptedId = _protector.Protect(newStudent.Id.ToString());

                return RedirectToAction("Details",new { id = encryptedId });
            }
            return View();

            #region 重构之前
            //if (ModelState.IsValid)
            //{
            //    string uniqueFileName = null;

            //    //单个图片上传
            //    //if (model.Photos!=null)
            //    //{
            //    //    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
            //    //    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photos.FileName;
            //    //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //    //    model.Photos.CopyTo(new FileStream(filePath, FileMode.Create));
            //    //}

            //    foreach (IFormFile photo in model.Photos)
            //    {
            //        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images","avatars");
            //        uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
            //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //        photo.CopyTo(new FileStream(filePath, FileMode.Create));
            //    }

            //    Student newStuent = new Student()
            //    {
            //        Name = model.Name,
            //        Email = model.Email,
            //        MaJor = model.MaJor,
            //        PhotoPath = uniqueFileName
            //    };
            //    _studentRepository.Insert(newStuent);

            //    return RedirectToAction("Details", new { id=newStuent.Id });

            //}
            //return View();
            #endregion
        }

        [HttpGet]
        public ViewResult Edit(string Id) 
        {
            var student = DecryptedStudent(Id);
            if (student == null)
            {
                ViewBag.ErrorMessage = $"学生Id={Id}的信息不存在，请重试。";
                return View("NotFound");
            }
            StudentEditViewModel studentEditViewModel = new StudentEditViewModel
            {
                Id = Id,
                Name = student.Name,
                Email = student.Email,
                MaJor = student.MaJor,
                ExistingPhotoPath = student.PhotoPath,
                EnrollmentDate = student.EnrollmentDate,
            };
            return View(studentEditViewModel);

            #region 

            //Student stu = _studentRepository.GetAll(Id);

            //StudentEditViewModel studentEditViewModel = new StudentEditViewModel()
            //{
            //    Id = stu.Id,
            //    Name = stu.Name,
            //    Email = stu.Email,
            //    MaJor = stu.MaJor,
            //    ExistingPhotoPath = stu.PhotoPath
            //};

            //return View(studentEditViewModel);

            #endregion
        }

        [HttpPost]
        public IActionResult Edit(StudentEditViewModel model)
        {

            //检查提供的数据是否有效，如果没有通过验证，则需要重新编辑学生信息
            //这样用户就可以更正并重新提交编辑表单
            if (ModelState.IsValid)
            {
                var student = DecryptedStudent(model.Id);

                //用模型对象中的数据更新Student对象
                student.Name = model.Name;
                student.Email = model.Email;
                student.MaJor = model.MaJor;
                student.EnrollmentDate = model.EnrollmentDate;

                //如果用户想要更改图片，可以上传新图片它会被模型对象上的Photo属性接收
                //如果用户没有上传图片，那么我们会保留现有的图片信息
                //因为兼容了多图上传，所有!=null判断修改判断Photos的总数是否大于0
                if (model.Photos != null && model.Photos.Count > 0)
                {
                    //如果上传了新的图片，则必须显示新的图片信息
                    //因此我们会检查当前学生信息中是否有图片，有的话，就会删除它
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath,"images","avatars",model.ExistingPhotoPath);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    /**我们将保存新的图片到wwwroot/images/avatars文件夹中，并且会更新Student对象中的PhotoPath属性，最终都会将它们保存到数据库中**/
                    student.PhotoPath = ProcessUploadedFile(model);
                }
                //调用仓储服务中的Update()方法，保存Studnet对象中的数据，更新数据
                //库表中的信息
                Student updatedstudent = _studentRepository.Update(student);
                return RedirectToAction("index");
            }
            return View(model);

            #region
            //if (ModelState.IsValid)
            //{
            //    Student stu = _studentRepository.GetStudent(model.Id);

            //    stu.Name = model.Name;
            //    stu.Email = model.Email;
            //    stu.MaJor = model.MaJor;

            //    //如果修改图片
            //    if (model.Photos!=null&& model.Photos.Count>0)
            //    {
            //        if (model.ExistingPhotoPath!=null)
            //        {
            //            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "avatars",model.ExistingPhotoPath);
            //            System.IO.File.Delete(filePath);
            //        }
            //        stu.PhotoPath= ProcessUploadFile(model);
            //    }

            //    Student updatedstudnet = _studentRepository.Update(stu);

            //    return RedirectToAction("Index");
            //}

            //return View(model);
            #endregion
        }

        public ViewResult Details(string Id)
        {
            //throw new Exception("在Details视图中抛出异常！！");

            //_logger.LogTrace("Trace（跟踪）log");
            //_logger.LogDebug("Debug（调试）log");
            //_logger.LogInformation("Information（信息）log");
            //_logger.LogWarning("Warning（警告）log");
            //_logger.LogError("Error（错误）log");
            //_logger.LogCritical("Critical（严重）log");

            var student = DecryptedStudent(Id);

            //判断学生信息是否存在
            if (student == null)
            {
                ViewBag.ErrorMessage = $"学生Id={Id}的信息不存在，请重试。";
                return View("NotFound");
            }
    
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Student = student,
                Title = "学生详情"
            };

            homeDetailsViewModel.Student.EncryptedId =
              _protector.Protect(student.Id.ToString());

            return View(homeDetailsViewModel);
        }

        public async Task<IActionResult> Detele(int Id)
        {
            var student = await _studentRepository.FirstOrDefaultAsync(a => a.Id == Id);


            if (student == null)
            {
                ViewBag.ErrorMessage = $"无法找到ID为{Id}的学生信息";
                return View("NotFound");
            }

            await _studentRepository.DeleteAsync(a => a.Id == Id);
            return RedirectToAction("Index");

            //var delStu = _studentRepository.Delete(Id);
            //return "删除成功";
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

        #region 私有方法

        /// <summary>
        /// 解密学生信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Student DecryptedStudent(string id)
        {
            //使用 Unprotect()方法来解析学生id
            string decryptedId = _protector.Unprotect(id);
            int decryptedStudentId = Convert.ToInt32(decryptedId);
            Student student = _studentRepository.FirstOrDefault(s => s.Id == decryptedStudentId);
            return student;
        }

        /// <summary>
        /// 将照片保存到指定的路径中，并返回唯一的文件名
        /// </summary>
        /// <returns></returns>
        private string ProcessUploadedFile(StudentCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photos != null && model.Photos.Count > 0)
            {
                foreach (var photo in model.Photos)
                {
                    //必须将图像上传到wwwroot中的images/avatars文件夹
                    //而要获取wwwroot文件夹的路径，我们需要注入 ASP.NET Core提供的webHostEnvironment服务
                    //通过webHostEnvironment服务去获取wwwroot文件夹的路径
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars");
                    //为了确保文件名是唯一的，我们在文件名后附加一个新的GUID值和一个下划线
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    //因为使用了非托管资源，所以需要手动进行释放
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        //使用IFormFile接口提供的CopyTo()方法将文件复制到wwwroot/images/avatars 文件夹
                        photo.CopyTo(fileStream);
                    }
                }
            }
            return uniqueFileName;
        }


        #endregion
    }
}