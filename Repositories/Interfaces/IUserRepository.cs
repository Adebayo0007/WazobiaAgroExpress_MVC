using Agro_Express.Models;

namespace Agro_Express.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
         Task<bool> ExistByEmailAsync(string userEmail);
         Task<bool> ExistByPasswordAsync(string userPassword);
        Task<IEnumerable<User>> SearchUserByEmailOrUsername(string searchInput); 
        Task<IEnumerable<User>> PendingRegistration(); 
        
    }
}