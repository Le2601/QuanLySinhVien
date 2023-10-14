
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using QuanLySinhVien.Models;
//using QuanLySinhVien.ModelView;
//using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace QuanLySinhVien.DI.Roles
{
    public interface IRoleRepository
    {

        //lay ra 1 cai
        Task<Role> GetRole(int id);

        //lay ra danh sach 
        Task<IEnumerable<Role>> GetRoles();

        Task<Role> CreateRole(Role model);

        //Task<department> UpdateDepartment(department model);

        //Task DeleteDepartment(int id);

       


    }
}
