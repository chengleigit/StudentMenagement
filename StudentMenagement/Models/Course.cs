using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentMenagement.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
