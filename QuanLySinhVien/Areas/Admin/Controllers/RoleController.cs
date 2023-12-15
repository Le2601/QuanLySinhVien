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
using Microsoft.EntityFrameworkCore;

namespace QuanLySinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
  
    public class RoleController : Controller
    {
        private readonly IRoleRepository _RolesRepository;
        private readonly ElearingDbContext _context;
        public RoleController(ElearingDbContext context, IRoleRepository roleRepository)
        {
            _context = context;
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
           if (ModelState.IsValid)
            {
                var CreateRole = await _RolesRepository.CreateRole(model);



                return RedirectToAction("index");
            }
            return View(model);


        }

        //public IActionResult Edit(int id)
        //{

        //    var item = _context.Roles.Where(x=> x.Id == id).FirstOrDefault();

        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(item);
        //}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            var item = await _context.Roles.Where(x=> x.Id == id).FirstOrDefaultAsync();

            //kiem tra neu khoa da ton tai khoa hoc

            var CheckAccount = _context.Account.Where(x => x.RoleId == id).ToList();
           

            if (CheckAccount.Count >= 1 )
            {
                return Json(new { success = false, msg = "Tồn tại khóa ngoại không thể xóa" });
            }



            if (item != null)
            {
               _context.Roles.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });


        }

    }
}
