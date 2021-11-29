using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentMenagement.Application.Courses;
using StudentMenagement.Application.Dtos;
using StudentMenagement.Infrastructure.Repositories;
using StudentMenagement.Models;
using StudentMenagement.ViewModels.Courses;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IRepository<Course, int> _courseRepository;
        private readonly IRepository<Department, int> _departmentRepository;
        private readonly IRepository<CourseAssignment, int> _courseAssignmentsRepository;

        public CourseController(ICourseService courseService,
            IRepository<Course, int> courseRepository,
            IRepository<Department, int> departmentRepository,
            IRepository<CourseAssignment, int> courseAssignmentsRepository)
        {
            _courseService = courseService;
            _courseRepository = courseRepository;
            _departmentRepository = departmentRepository;
            _courseAssignmentsRepository = courseAssignmentsRepository;
        }

        // 不填写 [HttpGet]默认为处理GET请求
        public async Task<ActionResult> Index(GetCourseInput input)
        {
            var models = await _courseService.GetPaginatedResult(input);
            return View(models);
        }

        #region 添加课程
        public ActionResult Create()
        {
            var dtos = DepartmentsDropDownList();
            CourseCreateViewModel courseCreateViewModel = new CourseCreateViewModel
            {
                DepartmentList = dtos
            };
            //将DepartmentsDropDownList()方法的SelectList返回值添加到courseCreateViewModel中，
            //传递到视图中
            return View(courseCreateViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CourseCreateViewModel input)
        {
            if (ModelState.IsValid)
            {
                Course course = new Course
                {
                    CourseID = input.CourseID,
                    Title = input.Title,
                    Credits = input.Credits,
                    DepartmentID = input.DepartmentID
                };

                await _courseRepository.InsertAsync(course);

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        #endregion

        #region 编辑功能
        [HttpGet]
        public IActionResult Edit(int? courseId)
        {
            if (!courseId.HasValue)
            {
                ViewBag.ErrorMessage = $"课程编号{courseId}的信息不存在，请重试。";
                return View("NotFound");
            }

            var course = _courseRepository.FirstOrDefault(a => a.CourseID == courseId);
            if (course == null)
            {
                ViewBag.ErrorMessage = $"课程编号{courseId}的信息不存在，请重试。";
                return View("NotFound");
            }
            var dtos = DepartmentsDropDownList(course.DepartmentID);//将学//院列表中选中的值修改为true
            CourseCreateViewModel courseCreateViewModel = new CourseCreateViewModel
            {
                DepartmentList = dtos,
                CourseID = course.CourseID,
                Credits = course.Credits,
                Title = course.Title,
                DepartmentID = course.DepartmentID
            };
            return View(courseCreateViewModel);
        }

        [HttpPost]
        public IActionResult Edit(CourseCreateViewModel input)
        {
            if (ModelState.IsValid)
            {
                var course = _courseRepository.FirstOrDefault(a => a.CourseID == input.CourseID);
                if (course != null)
                {
                    course.CourseID = input.CourseID;
                    course.Credits = input.Credits;
                    course.DepartmentID = input.DepartmentID;
                    course.Title = input.Title;
                    _courseRepository.Update(course);
                    return RedirectToAction(nameof(Index));//返回列表页面
                }
                else
                {
                    ViewBag.ErrorMessage = $"课程编号{input.CourseID}的信息不存在，请重试。";
                    return View("NotFound");
                }
            }
            return View(input);
        }

        #endregion

        #region 删除功能
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            var model = await _courseRepository.FirstOrDefaultAsync(a => a.CourseID == id);


            if (model == null)
            {
                ViewBag.ErrorMessage = $"课程编号{id}的信息不存在，请重试。";
                return View("NotFound");
            }

            await _courseAssignmentsRepository.DeleteAsync(a => a.CourseID == model.CourseID);
            await _courseRepository.DeleteAsync(a => a.CourseID == id);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        public async Task<ViewResult> Details(int courseId)
        {

            var course = await _courseRepository.GetAll().Include(a => a.Department).FirstOrDefaultAsync(a => a.CourseID == courseId);

            //判断学生信息是否存在
            if (course == null)
            {
                ViewBag.ErrorMessage = $"课程编号{courseId}的信息不存在，请重试。";
                return View("NotFound");
            }

            return View(course);
        }

        /// <summary>
        /// 学院的下拉列表
        /// </summary>
        /// <param name="selectedDepartment"> </param>
        private SelectList DepartmentsDropDownList(object selectedDepartment = null)
        {
            var models = _departmentRepository.GetAll().OrderBy(a => a.Name).AsNoTracking().ToList();
            var dtos = new SelectList(models, "DepartmentID", "Name", selectedDepartment);
            return dtos;
        }


    }
}
