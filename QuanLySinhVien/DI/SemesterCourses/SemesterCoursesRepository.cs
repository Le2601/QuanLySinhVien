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

namespace QuanLySinhVien.DI.SemesterCourses
{
    public class SemesterCoursesRepository : ISemesterCoursesRepository
    {

        private readonly ElearingDbContext _dbContext;

        public SemesterCoursesRepository(ElearingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SemesterCourse> GetSemesterCourse(int id)
        {
            return await _dbContext.SemesterCourse.FindAsync(id);
        }

        public async Task<IEnumerable<SemesterCourse>> GetSemesterCourses()
        {
            return await _dbContext.SemesterCourse.ToListAsync();
        }

        public async Task<SemesterCourse> Create(SemesterCourse model)
        {
            _dbContext.SemesterCourse.Add(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<SemesterCourse> Delete(SemesterCourse model)
        {

            _dbContext.SemesterCourse.Remove(model);
            await _dbContext.SaveChangesAsync();



            return model;
        }


    }
}
