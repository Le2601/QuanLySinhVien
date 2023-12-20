using System;
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
using QuanLySinhVien.Models;
//using QuanLySinhVien.ModelView;
using QuanLySinhVien.Areas.Admin.Models;

using QuanLySinhVien.Helpper;
using QuanLySinhVien.Extension;


using System.Text;

namespace QuanLySinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountsController : Controller
    {
        private readonly ElearingDbContext _context;

       

        public AccountsController(ElearingDbContext context)
        {
            _context = context;
           
        }

    

        // GET: Admin/Accounts
        public async Task<IActionResult> Index()
        {
            var elearingDbContext = _context.Account.Include(a => a.Role);

            ViewBag.RoleId = new SelectList(_context.Roles.ToList(), "Id", "RoleName");

            return View(await elearingDbContext.ToListAsync());
        }

        // GET: Admin/Accounts/Details/5
       

        // GET: Admin/Accounts/Create
        public IActionResult Create()
        {
            //ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id");


            ViewBag.RoleId = new SelectList(_context.Roles.ToList(), "Id", "RoleName");
            return View();
        }

        // POST: Admin/Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account model)
        {

            var ListCourse = _context.Account.Where(x => x.Email == model.Email).ToList();
            if (ModelState.IsValid)
            {

                if (ListCourse.Count > 0)
                {
                    ViewBag.RoleId = new SelectList(_context.Roles.ToList(), "Id", "RoleName");
                    ModelState.AddModelError("Email", "Email đã tồn tại.");
                   
                    return View(model);
                }

                string salt = Utilities.GetRandomKey();

                model.Password = (model.Password.Trim()).ToMD5();





                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", model.RoleId);
            ViewBag.RoleId = new SelectList(_context.Roles.ToList(), "Id", "RoleName");
            return View(model);
        }

        // GET: Admin/Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", account.RoleId);
            return View(account);
        }

        // POST: Admin/Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Account model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", model.RoleId);
            return View(model);
        }

        // GET: Admin/Accounts/Delete/5
        [HttpPost]
        public async Task<IActionResult> delete(int id)
        {
            var Account = await _context.Account.Where(x => x.Id == id).FirstAsync();

            //kiem tra neu khoa da ton tai khoa hoc

            var CheckCourse = _context.Courses.Where(x => x.AccountId == id).ToList();
           

            if (CheckCourse.Count >= 1 )
            {
                return Json(new { success = false, msg = "Tồn tại khóa ngoại không thể xóa" });
            }



            if (Account != null)
            {
                _context.Account.Remove(Account);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
       
        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.Id == id);
        }


        //login va logout

        // login admin


        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "Login")]

        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID != null)
            {
                return RedirectToAction("Index", "Home", new { Area = "Admin" });
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "Login")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (model.Password.Length <= 5)
            {
                ViewBag.Error = "Mật khẩu phải hơn 6 ký tự";
                return View(model);
            }
            try
            {
                if (ModelState.IsValid)
                {
                    Account kh = _context.Account.Include(a => a.Role)
                        .SingleOrDefault(a => a.Email.ToLower() == model.Email.ToLower().Trim());

                    if (kh == null)
                    {
                        ViewBag.Error = "Tài khoản không tồn tại";
                        return View(model);
                    }
                    string pass = (model.Password.Trim()).ToMD5();

                    if (kh.Password.Trim() != pass)
                    {
                        ViewBag.Error = "Sai mật khâu";
                        return View(model);
                    }
                    

                   if(kh.RoleId == 3)
                    {
                        //dang nhap thanh cong

                        //ghi nhan tg dang nhap
                        kh.LastLogin = DateTime.Now;
                        _context.Update(kh);
                        await _context.SaveChangesAsync();

                        var taikhoan = HttpContext.Session.GetString("AccountId");

                        //identity

                        //luu session makh

                        HttpContext.Session.SetString("AccountId", kh.Id.ToString());

                        //edentity

                        var userClaims = new List<Claim>
                    {

                        new Claim(ClaimTypes.Name, kh.FullName),
                        new Claim(ClaimTypes.Email, kh.Email),
                        new Claim("AccountId", kh.Id.ToString()),
                        new Claim("RoleId", kh.RoleId.ToString()),
                        new Claim(ClaimTypes.Role, kh.Role.RoleName)


                    };

                        var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                        var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                        await HttpContext.SignInAsync(userPrincipal);



                        return RedirectToAction("Index", "Home", new { Area = "Admin" });
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
                return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
            }

            return RedirectToAction("Login", "Accounts", new { Area = "Admin" });




        }


        //logout

        [Route("logout.html", Name = "Logout")]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("AccountId");
                return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
            }
            catch
            {
                return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
            }
        }

        //[ResponseCache(Duration = 60)] // Caching trong 60 giây
        public IActionResult ListStudents()
        {

            var item = _context.Account.Where(x=> x.RoleId == 5).ToList();
            return View(item);
        }
        //[ResponseCache(Duration = 60)] // Caching trong 60 giây
        public IActionResult ListLecturers()
        {
            var item = _context.Account.Where(x => x.RoleId == 4).ToList();

            return View(item);
        }
    }
}
