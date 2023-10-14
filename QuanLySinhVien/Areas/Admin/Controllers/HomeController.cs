using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
  
    [Authorize]
    public class HomeController : Controller
    {

        [Route("admin.html", Name = "AdminHome")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
