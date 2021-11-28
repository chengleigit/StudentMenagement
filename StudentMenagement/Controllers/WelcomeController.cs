using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentMenagement.Infrastructure.Repositories;
using StudentMenagement.Models;
using System.Threading.Tasks;

namespace StudentMenagement.Controllers
{
    public class WelComeController:Controller
    {
        private readonly IRepository<Student,int> _studentRepository;
        public WelComeController(IRepository<Student, int> studentRepository)
        {
            _studentRepository=studentRepository;
        }

        public async Task<string> Index()
        {
            var student = await _studentRepository.GetAll().FirstOrDefaultAsync();
            var oop = await _studentRepository.SingleAsync(a => a.Id == 3);

            var longCount = await _studentRepository.LongCountAsync();

            var count = _studentRepository.Count();

            return $"{oop.Name}+{student.Name}+{longCount}+{count}";
        }
    }
}
