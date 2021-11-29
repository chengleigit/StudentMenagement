namespace StudentMenagement.Application.Dtos
{
    public class GetCourseInput : PagedSortedAndFilterInput
    {
        public GetCourseInput()
        {
            Sorting = "CourseID";
            MaxResultCount = 3;
        }
    }
}
