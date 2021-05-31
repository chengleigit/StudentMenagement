using StudentMenagement.Infrastructure;
using StudentMenagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement.DataRepositories
{
    public class SQLStudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public SQLStudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public Student Delete(int Id)
        {
            var student = _context.Studnets.FirstOrDefault(i => i.Id == Id);
            if (student!=null)
            {
                _context.Remove(student);
                _context.SaveChanges();
            }
            return student;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _context.Studnets;
        }

        public Student GetStudent(int Id)
        {
            return _context.Studnets.Find(Id);
        }

        public Student Insert(Student student)
        {
            _context.Studnets.Add(student);
            _context.SaveChanges();
            return student;
        }

        public Student Update(Student updateStudent)
        {
            var student = _context.Studnets.Attach(updateStudent);
            student.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return updateStudent;
        }
    }
}
