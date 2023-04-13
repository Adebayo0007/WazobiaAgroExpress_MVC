using Agro_Express.Enum;
using Agro_Express.Models;

namespace Agro_Express.Repositories.Interfaces
{
    public interface IProductRepository 
    {
        Task<Product> CreateAsync(Product product);
        Product GetProductById(string productId);
        Task<IEnumerable<Product>> GetProductsByFarmerEmailAsync(string farmerEmail);
        Task<IEnumerable<Product>> GetAllFarmProductAsync();
         Task<IEnumerable<Product>> GetAllFarmProductByLocationAsync(LocalGovernment buyerLocalGovernment, User user);
        Task<IEnumerable<Product>> GetFarmerFarmProductsByIdAsync(string farmerId);
        Task<IEnumerable<Product>> SearchProductsByProductNameOrFarmerUserNameOrFarmerEmail(string searchInput, User user);
        Product UpdateProduct(Product product);
        Task DeleteProduct(Product product);        
        Task DeleteExpiredProducts();
    }
}