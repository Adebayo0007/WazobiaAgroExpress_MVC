using Agro_Express.ApplicationContext;
using Agro_Express.Models;
using Agro_Express.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agro_Express.Repositories.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AdminRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            
        }
        public Task<Admin> CreateAsync(Admin admin)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(Admin admin)
        {
            _applicationDbContext.Admins.Update(admin);
        }

        public async Task<IEnumerable<Admin>> GetAllAsync()
        {
           return await _applicationDbContext.Admins.Include(a => a.User).ThenInclude(a => a.Address).Where(a => a.User.IsActive == true && a.User.Role == "Admin").ToListAsync();
        }

        public Task<IEnumerable<Admin>> GetAllNonActiveAsync()
        {
            throw new NotImplementedException();
        }

        public Admin GetByEmailAsync(string adminEmail)
        {
             return _applicationDbContext.Admins.Include(a => a.User).ThenInclude(a => a.Address).SingleOrDefault(a => a.User.Email == adminEmail);
        }

        public Admin GetByIdAsync(string adminId)
        {
              return _applicationDbContext.Admins.Include(a => a.User).ThenInclude(a => a.Address).SingleOrDefault(a => a.Id == adminId);
        }

        public async Task SaveChangesAsync()
        {
           await _applicationDbContext.SaveChangesAsync();
            
        }

        public Admin Update(Admin admin)
        {
              _applicationDbContext.Admins.Update(admin);
              _applicationDbContext.SaveChanges();
            return admin;
        }
    }
}