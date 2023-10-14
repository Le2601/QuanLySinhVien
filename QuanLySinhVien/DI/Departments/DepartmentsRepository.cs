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

namespace QuanLySinhVien.DI.Departments
{
    public class DepartmentsRepository : IDepartmentsRepository
    {

        private readonly ElearingDbContext _dbContext;

        public DepartmentsRepository(ElearingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<department> GetDepartment(int id)
        {
            return await _dbContext.Department.FindAsync(id);
        }

        public async Task<IEnumerable<department>> GetDepartments()
        {
            return await _dbContext.Department.ToListAsync();


        }

        public async Task<department> CreateDepartment(department model)
        {
            _dbContext.Department.Add(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<department> Delete(department model)
        {
         
                _dbContext.Department.Remove(model);
                await _dbContext.SaveChangesAsync();

               
            
            return model;
        }





    }
}
