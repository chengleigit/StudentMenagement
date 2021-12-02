using StudentMenagement.Application.Departments.Dtos;
using StudentMenagement.Application.Dtos;
using StudentMenagement.Models;
using System.Threading.Tasks;

namespace StudentMenagement.Application.Departments
{
    public interface IDepartmentsService
    {
        /// <summary>
        /// 获取学院的分页信息
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        Task<PagedResultDto<Department>> GetPagedDepartmentsList(GetDepartmentInput input);
    }
}
