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

namespace QuanLySinhVien.DI.CourseMembers
{
    public class CourseMemberRepository : ICourseMemberRepository
    {
        private readonly ElearingDbContext _dbContext;

        //dung de lay duong dan rooot de luu du lieu upload
        private readonly IWebHostEnvironment _environment;

        public CourseMemberRepository(ElearingDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;

            _environment = environment;
        }

        public async Task<CourseMember> GetById(int id)
        {
            return await _dbContext.CourseMembers.FindAsync(id);
        }

        public async Task<IEnumerable<CourseMember>> GetAll()
        {
            return await _dbContext.CourseMembers.OrderBy(x => x.Id).ToListAsync();
        }



        public IEnumerable<Course> GetAllCourse()
        {
            return _dbContext.Courses.ToList();
        }



        public async Task<CourseMember> Create(CourseMember model)
        {
            _dbContext.CourseMembers.Add(model);
            await _dbContext.SaveChangesAsync();

            return model;
        }

        public async Task<CourseMember> Delete(CourseMember model,int id)
        {

            var Getid = await _dbContext.CourseMembers.FindAsync(id);

            if (Getid != null)
            {

                _dbContext.CourseMembers.Remove(Getid);
                await _dbContext.SaveChangesAsync();

              

            }
            return model;
        }


        public async Task<CourseMember> Update(CourseMember model)
        {
            _dbContext.CourseMembers.Update(model);
            await _dbContext.SaveChangesAsync();

            return model;
        }


    }
}
