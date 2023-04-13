using Agro_Express.Models;

namespace Agro_Express.Repositories.Interfaces
{
    public interface IBuyerRepository : IBaseRepository<Buyer>
    {
         Task<IEnumerable<Buyer>> SearchBuyerByEmailOrUsername(string searchInput);
         
    }
}