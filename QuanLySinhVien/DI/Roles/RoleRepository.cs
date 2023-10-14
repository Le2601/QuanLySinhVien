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

namespace QuanLySinhVien.DI.Roles
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ElearingDbContext _dbContext;

        public RoleRepository(ElearingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Role> GetRole(int id)
        {
            return await _dbContext.Roles.FindAsync(id);
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            return await _dbContext.Roles.ToListAsync();

           

        }

        public async Task<Role> CreateRole(Role model)
        {
            _dbContext.Roles.Add(model);
            await _dbContext.SaveChangesAsync();
            return model;
          
        }


    }
}
