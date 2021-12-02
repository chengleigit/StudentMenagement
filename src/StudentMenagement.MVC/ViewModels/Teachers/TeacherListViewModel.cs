using StudentMenagement.Application.Dtos;
using StudentMenagement.Models;
using System.Collections.Generic;

namespace StudentMenagement.ViewModels.Teachers
{
    public class TeacherListViewModel
    {
        public PagedResultDto<Teacher> Teachers { get; set; }
        public List<Course> Courses { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }
        /// <summary>
        /// 选中的教师ID
        /// </summary>
        public int SelectedId { get; set; }
        /// <summary>
        /// 选中的课程ID
        /// </summary>
        public int SelectedCourseId { get; set; }
    }
}
