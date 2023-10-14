using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using QuanLySinhVien.Models;
//using QuanLySinhVien.ModelView;
//using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using QuanLySinhVien.DI.Departments;

namespace QuanLySinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] //bat xac thuc trc khi vao trang nay
    public class DepartmentController : Controller
    {
        private readonly IDepartmentsRepository _departmentsRepository;

        public DepartmentController(IDepartmentsRepository departmentsRepository)
        {
            _departmentsRepository = departmentsRepository;
        }

        public async Task<ActionResult<IEnumerable<department>>> Index()
        {
            var items = await _departmentsRepository.GetDepartments();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(department model)
        
        {

            if (ModelState.IsValid)
            {
                model.Alias = QuanLySinhVien.Helpper.Utilities.SEOUrl(model.Title);
                var CreateDepartment = await _departmentsRepository.CreateDepartment(model);
            }
               

                

                return RedirectToAction("index");

        
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item =await _departmentsRepository.GetDepartment(id);

            if(item != null)
            {
               await _departmentsRepository.Delete(item);

                return Json(new { success = true });

            }

            return Json(new { success = false });
        }
    }
}
