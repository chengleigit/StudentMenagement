using StudentMenagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement.DataRepositories
{
    public interface IStudentRepository
    {
        Student GetStudent(int Id);

        IEnumerable<Student> GetAllStudents();
        Student Insert(Student student);

        Student Update(Student student);

        Student Delete(int Id);
    }
}
