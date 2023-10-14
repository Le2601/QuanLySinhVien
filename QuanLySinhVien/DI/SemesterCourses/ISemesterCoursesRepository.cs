using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using QuanLySinhVien.Models;
//using QuanLySinhVien.ModelView;
//using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace QuanLySinhVien.DI.SemesterCourses
{
    public interface ISemesterCoursesRepository
    {

        //lay ra 1 cai
        Task<SemesterCourse> GetSemesterCourse(int id);

        //lay ra danh sach 
        Task<IEnumerable<SemesterCourse>> GetSemesterCourses();

        Task<SemesterCourse> Create(SemesterCourse model);

        //Task<department> Update(department model);

        Task<SemesterCourse> Delete(SemesterCourse model);

    }
}
