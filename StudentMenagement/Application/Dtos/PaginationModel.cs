using StudentMenagement.Models;
using System;
using System.Collections.Generic;

namespace StudentMenagement.Application.Dtos
{
    public class PaginationModel
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; } = 1;
        /// <summary>
        /// 总条数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 每页分页条数
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count,PageSize));

        public List<Student> Data { get; set; }


        public bool ShowPrevious => CurrentPage > 1;
        public bool ShowNext => CurrentPage < TotalPages;

        public bool ShowFirst => CurrentPage != 1;
        public bool ShowLast => CurrentPage != TotalPages;

    }
}
