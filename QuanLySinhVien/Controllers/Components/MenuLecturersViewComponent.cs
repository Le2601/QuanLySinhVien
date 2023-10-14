using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuanLySinhVien.Models;
//using WebTinTuc.Extension;
using QuanLySinhVien.ModelView;

namespace QuanLySinhVien.Controllers.Components
{
    public class MenuLecturersViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
         
            return View();
        }

    }
}
