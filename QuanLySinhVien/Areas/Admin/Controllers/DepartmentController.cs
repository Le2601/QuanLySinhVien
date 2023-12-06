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
    //[Authorize(Roles = "Employee,Admin")]
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly ElearingDbContext _context;
        public DepartmentController(ElearingDbContext context,IDepartmentsRepository departmentsRepository)
        {
            _context = context;
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

                return RedirectToAction("index");
            }

            return View(model);
               

                

                

        
        }

        public async Task<IActionResult> Edit(int id)
        {
             var get_items = await _departmentsRepository.GetDepartment(id);

           

           return View(get_items);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(department model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Alias = QuanLySinhVien.Helpper.Utilities.SEOUrl(model.Title);

            var UpdateDepartment = await _departmentsRepository.UpdateDepartment(model);

            return RedirectToAction("index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item =await _departmentsRepository.GetDepartment(id);

            //kiem tra neu khoa da ton tai khoa hoc

            var CheckCourse = _context.Courses.Where(x => x.DepartmentId == id).ToList();

            if( CheckCourse.Count >= 1) 
            {
                return Json(new { success = false,msg = "Tồn tại khóa ngoại không thể xóa" });
            }


            if(item != null)
            {
               await _departmentsRepository.Delete(item);

                return Json(new { success = true, msg = "Xóa thành công" });

            }

            return Json(new { success = false, msg = "Không tồn tại" });
        }
    }
}
