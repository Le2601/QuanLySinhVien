using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using QuanLySinhVien.Models;
//using QuanLySinhVien.ModelView;
//using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using QuanLySinhVien.Helpper;
using Microsoft.AspNetCore.Hosting;
using System;

namespace QuanLySinhVien.DI.Courses
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ElearingDbContext _dbContext;

        //dung de lay duong dan rooot de luu du lieu upload

        private readonly IWebHostEnvironment _environment;

        public CourseRepository(ElearingDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;

            _environment = environment;
        }

        public async Task<Course> GetCourse(int id)
        {
            return await _dbContext.Courses.FindAsync(id);
        }

        public async Task<IEnumerable<Course>> GetCourses()
        {
            return await _dbContext.Courses.ToListAsync();



        }

    

        public IEnumerable<department> GetDepartments()
        {
            return _dbContext.Department.ToList();
        }
        
          public IEnumerable<SemesterCourse> GetSemesterCourses()
        {
            return _dbContext.SemesterCourse.ToList();
        }

        public async Task<Course> CreateCourse(Course model)
        {
            _dbContext.Courses.Add(model);
            await _dbContext.SaveChangesAsync();
            return model;

        }

      


        public async Task<Course> EditCourse(Course model)
        {

            _dbContext.Courses.Update(model);
            await _dbContext.SaveChangesAsync();

            return model;

        }



        public async Task<Course> DeleteCourse(Course model)
        {
            _dbContext.Courses.Remove(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }


      

      
        

        //public IEnumerable<Account> GetNameCreator(int id)
        //{
        //    return _dbContext.Account.Where(x => x.Id == id).ToList();
        //}

        public IEnumerable<Account> GetAllCreator()
        {
            return _dbContext.Account.Where(x=> x.RoleId ==4).ToList();
        }

        

    }
}
