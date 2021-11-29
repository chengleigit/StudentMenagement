using StudentMenagement.Application.Dtos;
using StudentMenagement.Application.Teachers.Dtos;
using StudentMenagement.Models;
using System.Threading.Tasks;

namespace StudentMenagement.Application.Teachers
{
    public interface ITeacherService
    {
        /// <summary>
        /// 获取教师的分页信息
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        Task<PagedResultDto<Teacher>> GetPagedTeacherList(GetTeacherInput input);
    }
}
