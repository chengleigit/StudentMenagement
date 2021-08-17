using StudentMenagement.Models;
using System.Collections.Generic;

namespace StudentMenagement.DataRepositories
{
    public interface IStudentRepository
    {
        /// <summary>
        /// 获取学生信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Student GetStudent(int Id);
        /// <summary>
        /// 获取所有学生信息 
        /// </summary>
        /// <returns></returns>
        IEnumerable<Student> GetAllStudents();
        /// <summary>
        /// 增加学生信息
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        Student Insert(Student student);
        /// <summary>
        /// 更新学生信息
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        Student Update(Student student);
        /// <summary>
        /// 删除学生信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Student Delete(int Id);
    }
}