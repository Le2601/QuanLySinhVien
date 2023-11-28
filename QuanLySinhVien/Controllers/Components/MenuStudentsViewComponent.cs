using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuanLySinhVien.Models;
//using WebTinTuc.Extension;
using QuanLySinhVien.ModelView;


namespace QuanLySinhVien.Controllers.Components
{
    public class MenuStudentsViewComponent : ViewComponent
    {
        private readonly ElearingDbContext _context;

        public MenuStudentsViewComponent(ElearingDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            //lay id cua account dc luu vao session khi login

            string loginAdmin = HttpContext.Session.GetString("AccountId_Student");

            int IdAccount = int.Parse(loginAdmin);

            ViewBag.IdAccount = IdAccount;

           var item = _context.Account.Where(x=> x.Id == IdAccount).FirstOrDefault();


            var mssv = item.Code;

            

            

            var items = _context.CourseMembers.Where(x=> x.Mssv == mssv).ToList();

            ViewBag.Course = new SelectList(_context.Courses.ToList(), "Id", "Title");



            return View(items);
        }

    }
}
