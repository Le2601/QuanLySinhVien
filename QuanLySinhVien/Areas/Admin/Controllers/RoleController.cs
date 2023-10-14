using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using QuanLySinhVien.Models;
//using QuanLySinhVien.ModelView;
//using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using QuanLySinhVien.DI.Roles;
using QuanLySinhVien.DI.Departments;

namespace QuanLySinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class RoleController : Controller
    {
        private readonly IRoleRepository _RolesRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _RolesRepository = roleRepository;
        }

        public async Task<ActionResult<IEnumerable<Role>>> Index()
        {
            var items =await _RolesRepository.GetRoles();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Role model)

        {
            var CreateRole =await _RolesRepository.CreateRole(model);
            


            return RedirectToAction("index");


        }
       
    }
}
