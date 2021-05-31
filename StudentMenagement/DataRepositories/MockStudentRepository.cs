using StudentMenagement.Models;
using StudentMenagement.Models.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement.DataRepositories
{
    public class MockStudentRepository : IStudentRepository
    {
        private readonly List<Student> _student;
        public MockStudentRepository()
        {
            _student = new List<Student>()
            {
                new Student(){ Id=1, Name="张三", MaJob=MaEnum.FirstGrade, Email="168@qq.com" },
                new Student(){ Id=2, Name="李四", MaJob=MaEnum.SecondGrade, Email="168@qq.com" },
                new Student(){ Id=3, Name="王五", MaJob=MaEnum.GradeThree, Email="168@qq.com" }
            };
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _student;
        }

        public Student GetStudent(int Id)
        {
            return _student.FirstOrDefault(i => i.Id == Id);
        }

        public Student Insert(Student student)
        {
            student.Id = _student.Max(s=>s.Id) + 1;
            _student.Add(student);
            return student;
        }


        public Student Update(Student student)
        {
            Student studnet = _student.FirstOrDefault(i=>i.Id==student.Id);

            if (studnet!=null)
            {
                studnet.Name = student.Name;
                studnet.MaJob = student.MaJob;
                studnet.Email = student.Email;
            }

            return student;
        }

        public Student Delete(int Id)
        {
            var student = _student.FirstOrDefault(i=>i.Id==Id);
            if (student!=null)
            {
                _student.Remove(student);
            }
            return student;
        }
    }
}
