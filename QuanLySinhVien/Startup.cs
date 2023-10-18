using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuanLySinhVien.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using QuanLySinhVien.DI.Departments;
using QuanLySinhVien.DI.Roles;
using QuanLySinhVien.DI.Courses;
using Microsoft.Extensions.FileProviders;
using System.IO;
using QuanLySinhVien.DI.CourseMembers;
using QuanLySinhVien.DI.SemesterCourses;

namespace QuanLySinhVien
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));

            services.AddControllersWithViews().AddRazorRuntimeCompilation();



            //connect db
            services.AddDbContext<ElearingDbContext>(options => options.UseSqlServer(
                
                Configuration.GetConnectionString("ElearningConnection")

                ));


            services.AddSession();

            //Authentication
            //kiem tra neu chua dang nhap ma vao bang duong dan url thi se tra ve trang dang nhap
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(p =>
               {
                   p.Cookie.Name = "UserLoginCookie";
                   //xet tg ton tai cua phien dang nhap
                   p.ExpireTimeSpan = TimeSpan.FromHours(1);
                   p.LoginPath = "/login-student.html";
                   //p.LogoutPath = "/dang-xuat/html";
                   //p.AccessDeniedPath = "/not-found.html";
               });


            // Cấu hình DI
            services.AddScoped<IDepartmentsRepository, DepartmentsRepository>();

            //kỹ thuật oage caching  lưu trữ vào bộ nhớ cache giảm tg phản hồi
            services.AddResponseCaching();


            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<ICourseRepository, CourseRepository>();


            services.AddScoped<ICourseMemberRepository, CourseMemberRepository>();


            services.AddScoped<ISemesterCoursesRepository, SemesterCoursesRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseResponseCaching();


            app.UseStaticFiles();

            app.UseRouting();

           
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
                });


                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

               
                        });
        }
    }
}
