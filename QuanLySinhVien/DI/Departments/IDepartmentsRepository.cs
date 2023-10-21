using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using QuanLySinhVien.Models;
//using QuanLySinhVien.ModelView;
//using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace QuanLySinhVien.DI.Departments
{
    public interface IDepartmentsRepository
    {

        //lay ra 1 cai
        Task<department> GetDepartment(int id);

        //lay ra danh sach 
        Task<IEnumerable<department>> GetDepartments();

        Task<department> CreateDepartment(department model);

        Task<department> UpdateDepartment(department model);

        Task<department> Delete(department model);


    }
}
