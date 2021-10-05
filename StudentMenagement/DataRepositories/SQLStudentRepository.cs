using Microsoft.Extensions.Logging;
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
        private readonly ILogger<SQLStudentRepository> _logger;


        public SQLStudentRepository(AppDbContext context, ILogger<SQLStudentRepository> logger)
        {
            _context = context;
            _logger = logger;
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
            _logger.LogTrace("学生信息 Trace（跟踪）log");
            _logger.LogDebug("学生信息 Debug（调试）log");
            _logger.LogInformation("学生信息 Information（信息）log");
            _logger.LogWarning("学生信息 Warning（警告）log");
            _logger.LogError("学生信息 Error（错误）log");
            _logger.LogCritical("学生信息 Critical（严重）log");

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
