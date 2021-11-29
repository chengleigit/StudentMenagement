using StudentMenagement.Application.Dtos;

namespace StudentMenagement.Application.Departments.Dtos
{
    public class GetDepartmentInput:PagedSortedAndFilterInput
    {
        public GetDepartmentInput()
        {
            Sorting = "Name";
            MaxResultCount = 3;
        }
    }
}
