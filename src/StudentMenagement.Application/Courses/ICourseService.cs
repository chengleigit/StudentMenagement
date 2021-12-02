using StudentMenagement.Application.Dtos;
using StudentMenagement.Models;
using System.Threading.Tasks;

namespace StudentMenagement.Application.Courses
{
    public interface ICourseService
    {
        Task<PagedResultDto<Course>> GetPaginatedResult(GetCourseInput input);
    }
}
