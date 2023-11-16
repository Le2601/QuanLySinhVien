﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Extension;
using QuanLySinhVien.Models;
using QuanLySinhVien.ModelView;


namespace QuanLySinhVien.Controllers.Lecturers
{
   
    [Authorize(Roles = "Employee")]
    public class AccountLecturersController : Controller
    {
        private readonly ElearingDbContext _context;

        public AccountLecturersController(ElearingDbContext context)
        {
            _context = context;
        }

        //login va logout

        // login admin


        [AllowAnonymous]
        [Route("login-giangvien.html", Name = "Login-giangvien")]

        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId_Lecturers");
            if (taikhoanID != null)
            {
                return RedirectToAction("Index", "HomeLecturers");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login-giangvien.html", Name = "Login-giangvien")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Account kh = _context.Account.Include(a => a.Role)
                        .SingleOrDefault(a => a.Email.ToLower() == model.Email.ToLower().Trim());

                    if (kh.Email == null)
                    {
                        ViewBag.Error = "Thông tin không thể thiếu";
                        return View(model);
                    }

                    string pass = (model.Password.Trim()).ToMD5();

                    if (kh.Password.Trim() != pass)
                    {
                        ViewBag.Error = "Mật khẩu không đúng";
                        return View(model);
                    }


                    if (kh.RoleId == 4)
                    {
                        //dang nhap thanh cong

                        //ghi nhan tg dang nhap
                        kh.LastLogin = DateTime.Now;
                        _context.Update(kh);
                        await _context.SaveChangesAsync();

                        var taikhoan = HttpContext.Session.GetString("AccountId_Lecturers");

                        //identity

                        //luu session makh

                        HttpContext.Session.SetString("AccountId_Lecturers", kh.Id.ToString());

                        //edentity

                        var userClaims = new List<Claim>
                    {

                        new Claim(ClaimTypes.Name, kh.FullName),
                        new Claim(ClaimTypes.Email, kh.Email),
                        new Claim("AccountId_Lecturers", kh.Id.ToString()),
                        new Claim("RoleId", kh.RoleId.ToString()),
                        new Claim(ClaimTypes.Role, kh.Role.RoleName)


                    };

                        var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                        var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                        await HttpContext.SignInAsync(userPrincipal);



                        return RedirectToAction("Index", "HomeLecturers");

                    }
                    else
                    {
                        ViewBag.Error = "Tài khoản giảng viên không tồn tại";
                        return View(model);
                    }


                }
            }
            catch
            {
                return RedirectToAction("Login", "AccountLecturers");
            }

            return RedirectToAction("Login", "AccountLecturers");




        }


        //logout

        [Route("logout-giangvien.html", Name = "Logout-giangvien")]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("AccountId_Lecturers");
                return RedirectToAction("Login", "AccountLecturers");
            }
            catch
            {
                return RedirectToAction("Login", "AccountLecturers");
            }
        }
    }
}
