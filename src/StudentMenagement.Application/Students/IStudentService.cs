using StudentMenagement.Application.Dtos;
using StudentMenagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentMenagement.Application.Students
{
    public interface IStudentService
    {
        Task<PagedResultDto<Student>> GetPaginatedResult(GetStudentInput input);
    }
}
