using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentMenagement.Application.Departments;
using StudentMenagement.Application.Departments.Dtos;
using StudentMenagement.Infrastructure;
using StudentMenagement.Infrastructure.Repositories;
using StudentMenagement.Models;
using StudentMenagement.ViewModels.Departments;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IRepository<Department, int> _departmentRepository;
        private readonly IRepository<Teacher, int> _teacherRepository;
        private readonly IDepartmentsService _departmentsService;
        private readonly AppDbContext _dbcontext;

        public DepartmentsController(
            IRepository<Department, int> departmentRepository,
            IDepartmentsService departmentsService,
            IRepository<Teacher, int> teacherRepository,
            AppDbContext dbcontext)
        {
            _departmentRepository = departmentRepository;
            _departmentsService = departmentsService;
            _teacherRepository = teacherRepository;
            _dbcontext = dbcontext;
        }

        public async Task<IActionResult> Index(GetDepartmentInput input)
        {
            var models = await _departmentsService.GetPagedDepartmentsList(input);
            return View(models);
        }

        #region 添加

        public IActionResult Create()
        {
            var dto = new DepartmentCreateViewModel
            {
                TeacherList = TeachersDropDownList()
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentCreateViewModel input)
        {
            if (ModelState.IsValid)
            {
                Department model = new Department
                {
                    StartDate = input.StartDate,
                    DepartmentID = input.DepartmentID,
                    TeacherID = input.TeacherID,
                    Budget = input.Budget,
                    Name = input.Name
                };
                await _departmentRepository.InsertAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        #endregion


        public async Task<IActionResult> Details(int Id)
        {

            string query = "SELECT * FROM dbo.Departments WHERE DepartmentID={0}";
            var model = await _dbcontext.Departments.FromSqlRaw(query,Id).Include(d => d.Administrator)
                      .AsNoTracking()
                      .FirstOrDefaultAsync();

            ////因为需要实现预加载，所以不能直接使用FirstOrDefaultAsync()方法
            //var model = await _departmentRepository.GetAll().Include(a => a.Administrator).FirstOrDefaultAsync(a => a.DepartmentID == Id);
            //判断学院信息是否存在
            if (model == null)
            {
                ViewBag.ErrorMessage = $"部门ID{Id}的信息不存在，请重试。";
                return View("NotFound");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _departmentRepository.FirstOrDefaultAsync(a => a.DepartmentID == id);

            if (model == null)
            {
                ViewBag.ErrorMessage = $"学院编号{id}的信息不存在，请重试。";
                return View("NotFound");
            }
            await _departmentRepository.DeleteAsync(a => a.DepartmentID == id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _departmentRepository.GetAll().Include(a => a.Administrator).AsNoTracking()
            .FirstOrDefaultAsync(a => a.DepartmentID == id);
            if (model == null)
            {
                ViewBag.ErrorMessage = $"教师{id}的信息不存在，请重试。";
                return View("NotFound");
            }
            var teacherList = TeachersDropDownList();
            var dto = new DepartmentCreateViewModel
            {
                DepartmentID = model.DepartmentID,
                Name = model.Name,
                Budget = model.Budget,
                StartDate = model.StartDate,
                TeacherID = model.TeacherID,
                Administrator = model.Administrator,
                RowVersion = model.RowVersion,
                TeacherList = teacherList
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentCreateViewModel input)
        {
            if (ModelState.IsValid)
            {
                var model = await _departmentRepository.GetAll().Include(a => a.Administrator).FirstOrDefaultAsync(a => a.DepartmentID == input.DepartmentID);

                if (model == null)
                {
                    ViewBag.ErrorMessage = $"教师{input.DepartmentID}的信息不存在，请重试。";
                    return View("NotFound");
                }

                model.DepartmentID = input.DepartmentID;
                model.Name = input.Name;
                model.Budget = input.Budget;
                model.StartDate = input.StartDate;
                model.TeacherID = input.TeacherID;
                await _departmentRepository.UpdateAsync(model);

                //获取
                _dbcontext.Entry(model).Property("RowVersion").OriginalValue = input.RowVersion;
                try
                {
                    await _departmentRepository.UpdateAsync(model);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department)exceptionEntry.Entity;

                    //从数据库中获取Department实体中的RowVersion属性，然后将
                    //input.RowVersion赋值到OriginalValue中，EF Core会对两个值进行比较
                    _dbcontext.Entry(model).Property("RowVersion").OriginalValue = input.RowVersion;

                    try
                    {   //UpdateAsync()方法执行SaveChangesAsync()方法时，如果检测
                        //到并发冲突，则会触发DbUpdateConcurrencyException异常
                        await _departmentRepository.UpdateAsync(model);
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException e)
                    {   //触发异常后，获取异常的实体
                        exceptionEntry = e.Entries.Single();
                        clientValues = (Department)exceptionEntry.Entity;
                        //从数据库中获取该异常实体信息
                        var databaseEntry = exceptionEntry.GetDatabaseValues();
                        if (databaseEntry == null)
                        {//如果异常实体为null，则表示该行数据已经被删除
                            ModelState.AddModelError(string.Empty, "无法进行数据的修改。该部门信息已经被其他人所删除!");
                        }
                        else
                        {     //将异常实体中的错误信息精确到具体字段并传递到视图中
                            var databaseValues = (Department)databaseEntry.ToObject();
                            if (databaseValues.Name != clientValues.Name)
                                ModelState.AddModelError("Name", $"当前值:{databaseValues.Name}");
                            if (databaseValues.Budget != clientValues.Budget)
                                ModelState.AddModelError("Budget", $"当前值:{databaseValues.Budget}");
                            if (databaseValues.StartDate != clientValues.StartDate)
                                ModelState.AddModelError("StartDate", $"当前值:{databaseValues.StartDate}");
                            if (databaseValues.TeacherID != clientValues.TeacherID)
                            {
                                var teacherEntity =
                                     await _teacherRepository.FirstOrDefaultAsync(a => a.Id == databaseValues.TeacherID);
                                ModelState.AddModelError("TeacherId", $"当前值:{teacherEntity?.Name}");
                            }
                            ModelState.AddModelError("", "您正在编辑的记录已经被其他用户所修改，编辑操作已经被取消，数据库当前的值已经显示在页面上。请再次单击保存。否则请返回列表。");
                            input.RowVersion = databaseValues.RowVersion;
                            //记得初始化教师列表
                            input.TeacherList = TeachersDropDownList();
                            ModelState.Remove("RowVersion");
                        }
                    }
                }
            }
                return View(input);
            
        }


        /// <summary>
        /// 教师的下拉列表
        /// </summary>
        /// <param name="selectedTeacher"> </param>
        private SelectList TeachersDropDownList(object selectedTeacher = null)
        {
            var models = _teacherRepository.GetAll().OrderBy(a => a.Name).AsNoTracking().ToList();
            var dtos = new SelectList(models, "Id", "Name", selectedTeacher);
            return dtos;
        }


        #region EF Core 中加载
        
        /// <summary>
        /// 显示加载
        /// </summary>
        /// <returns></returns>
        public async Task<string> LoadData(int id=1) 
        {
            
            var department = _dbcontext.Departments
                    .Single(b => b.DepartmentID == id);

            //Collection()方法，实体与另外一个Collection集合之间的关联关系
            _dbcontext.Entry(department)
                    .Collection(b => b.Courses)
                    .Load();

            //  Reference()方法，可用于两个实体之间的关联
            _dbcontext.Entry(department)
                    .Reference(b => b.Administrator)
                    .Load();

            return "OK";
        }


        #endregion
    }
}
