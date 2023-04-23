using Agro_Express.Models;

namespace Agro_Express.Repositories.Interfaces
{
    public interface IFarmerRepository : IBaseRepository<Farmer>
    {
        Task<IEnumerable<Farmer>> SearchFarmerByEmailOrUsername(string searchInput); 
        Task<Farmer> GetFarmer(string userId); 
        Task FarmerMonthlyDueUpdate();
    }
}