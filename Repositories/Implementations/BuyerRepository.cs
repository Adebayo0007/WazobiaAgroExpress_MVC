using Agro_Express.ApplicationContext;
using Agro_Express.Models;
using Agro_Express.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agro_Express.Repositories.Implementations
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public BuyerRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            
        }
        public async  Task<Buyer> CreateAsync(Buyer buyer)
        {
              await _applicationDbContext.Buyers.AddAsync(buyer);
              await SaveChangesAsync();
              return buyer;
        }

        public async Task Delete(Buyer buyer)
        {
            _applicationDbContext.Buyers.Update(buyer);
        }

        public async Task<IEnumerable<Buyer>> GetAllAsync()
        {
               return await _applicationDbContext.Buyers.Include(a => a.User).ThenInclude(a => a.Address).Where(a => a.User.IsActive == true && a.User.Role == "Buyer").ToListAsync();
        }

        public async Task<IEnumerable<Buyer>> GetAllNonActiveAsync()
        {
            return await _applicationDbContext.Buyers.Include(a => a.User).ThenInclude(a => a.Address).Where(a => a.User.IsActive == false && a.User.Role == "Buyer").ToListAsync();
        }

        public Buyer GetByEmailAsync(string buyerEmail)
        {
            return  _applicationDbContext.Buyers.Include(a => a.User).ThenInclude(a => a.Address).SingleOrDefault(a => a.User.Email == buyerEmail);
        }

        public Buyer GetByIdAsync(string buyerId)
        {
            return _applicationDbContext.Buyers.Include(a => a.User).ThenInclude(a => a.Address).SingleOrDefault(a => a.Id == buyerId);
        }

        public async Task SaveChangesAsync()
        {
           await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Buyer>> SearchBuyerByEmailOrUsername(string searchInput)
        {
            var input = searchInput.ToLower().Trim();
            var searchedOutput = await _applicationDbContext.Buyers.Include(b => b.User).ThenInclude(b => b.Address).Where(b => b.User.Email  == input || b.User.UserName == input).ToListAsync();
            return searchedOutput;
        }

        public Buyer Update(Buyer buyer)
        {
             _applicationDbContext.Buyers.Update(buyer);
             _applicationDbContext.SaveChanges();
            return buyer;
        }
    }
}