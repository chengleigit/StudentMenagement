using StudentMenagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentMenagement.Application.Students
{
    public interface IStudentService
    {
        Task<List<Student>> GetPaginatedResult(int currentPage, string searchString, string sortBy, int pageSize = 10);
    }
}
