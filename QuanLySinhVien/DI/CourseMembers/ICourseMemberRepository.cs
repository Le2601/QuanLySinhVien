using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using QuanLySinhVien.Models;
//using QuanLySinhVien.ModelView;
//using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace QuanLySinhVien.DI.CourseMembers
{
    public interface ICourseMemberRepository
    {
        //phuong thuc
        //Từ khóa `task` được sử dụng để định nghĩa một phương thức có kiểu trả về là một `Task`.
        //`Task` là một đối tượng giúp thực hiện công việc bất đồng bộ 
        Task<CourseMember> GetById(int id);

        Task<IEnumerable<CourseMember>> GetAll();

        IEnumerable<Course> GetAllCourse();

        Task<CourseMember> Create(CourseMember model);


        Task<CourseMember> Update(CourseMember model);

        Task<CourseMember> Delete(CourseMember model,int id);
    }
}
