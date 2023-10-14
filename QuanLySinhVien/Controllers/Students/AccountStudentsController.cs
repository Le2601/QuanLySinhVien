using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuanLySinhVien.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
//using QuanLySinhVien.Models;
using QuanLySinhVien.ModelView;

using System.Text;
using QuanLySinhVien.Extension;

namespace QuanLySinhVien.Controllers.Students
{
    [Authorize(Roles = "Student")]
    public class AccountStudentsController : Controller
    {
        private readonly ElearingDbContext _context;

        public AccountStudentsController(ElearingDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        [Route("login-student.html", Name = "Login-student")]
        
        public IActionResult Login(string returnUrl = null)
        {
            //neu da dang nhap roi thi vao thang trang home
            var taikhoanID = HttpContext.Session.GetString("AccountId_Student");
            if (taikhoanID != null)
            {
                return RedirectToAction("Index", "HomeStudent");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login-student.html", Name = "Login-student")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Account kh = _context.Account.Include(a => a.Role)
                        .SingleOrDefault(a => a.Email.ToLower() == model.Email.ToLower().Trim());

                  


                    if (kh == null )
                    {
                        ViewBag.Error = "Thong tin dang nhap chua chinh xac";
                        return View(model);
                    }
                    string pass = (model.Password.Trim()).ToMD5();

                    if (kh.Password.Trim() != pass)
                    {
                        ViewBag.Error = "Thong tin dang nhap chua chinh xac";
                        return View(model);
                    }
                    if (kh != null && kh.RoleId == 5)
                    {
                        //dang nhap thanh cong

                        //ghi nhan tg dang nhap
                        kh.LastLogin = DateTime.Now;
                        _context.Update(kh);
                        await _context.SaveChangesAsync();

                        var taikhoan = HttpContext.Session.GetString("AccountId_Student");

                        //identity

                        //luu session makh

                        HttpContext.Session.SetString("AccountId_Student", kh.Id.ToString());

                        //edentity

                        var userClaims = new List<Claim>
                    {

                        new Claim(ClaimTypes.Name, kh.FullName),
                        new Claim(ClaimTypes.Email, kh.Email),
                        new Claim("AccountId_Student", kh.Id.ToString()),
                        new Claim("RoleId", kh.RoleId.ToString()),
                        new Claim(ClaimTypes.Role, kh.Role.RoleName)


                    };

                        var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                        var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                        await HttpContext.SignInAsync(userPrincipal);



                        return RedirectToAction("Index", "HomeStudent");
                    }






                }
            }
            catch
            {
                return RedirectToAction("Login", "AccountStudents", new { area = "", routeName = "Login-student" });
            }

            return RedirectToAction("Login", "AccountStudents", new { area = "", routeName = "Login-student" });




        }

        //logout
        
        [Route("logout-student.html", Name = "Logout-student")]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("AccountId_Student");
                return RedirectToAction("Login", "AccountStudents");
            }
            catch
            {
                return RedirectToAction("Login", "AccountStudents");
            }
        }

    }
}
