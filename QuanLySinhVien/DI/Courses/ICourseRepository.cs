
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using QuanLySinhVien.Models;
//using QuanLySinhVien.ModelView;
//using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace QuanLySinhVien.DI.Courses
{
    public interface ICourseRepository
    {

        //lay ra 1 cai
        Task<Course> GetCourse(int id);

        //lay ra danh sach 
        Task<IEnumerable<Course>> GetCourses();

        //lay ra danh sach khoa
        Task<Course> CreateCourse(Course model);

      

        IEnumerable<department> GetDepartments();

        IEnumerable<SemesterCourse> GetSemesterCourses();

        Task<Course> EditCourse(Course model);

       
        Task<Course> DeleteCourse(Course model);

        //lay ra ten cua nguoi tao course dua vao account
    

        //IEnumerable<Account> GetNameCreator(int id);

        IEnumerable<Account> GetAllCreator();

      





    }
}
