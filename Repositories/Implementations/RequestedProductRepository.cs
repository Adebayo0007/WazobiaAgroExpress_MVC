using Agro_Express.ApplicationContext;
using Agro_Express.Models;
using Agro_Express.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agro_Express.Repositories.Implementations
{
    public class RequestedProductRepository : IRequestedProductRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public RequestedProductRepository(ApplicationDbContext applicationDbContext) =>
            _applicationDbContext = applicationDbContext;
        public async Task<RequestedProduct> CreateAsync(RequestedProduct product)
        {
             var requestedProduct = await _applicationDbContext.RequestedProducts.AddAsync(product);
              await _applicationDbContext.SaveChangesAsync();
            return product;
        }

        public async Task DeleteRequestedProduct(RequestedProduct requestedProduct)
        {
            _applicationDbContext.RequestedProducts.Remove(requestedProduct);
            _applicationDbContext.SaveChanges();
        }

        public async Task<IEnumerable<RequestedProduct>> GetOrderedProduct(string buyerEmail) =>
            await _applicationDbContext.RequestedProducts
            .Where(r => r.BuyerEmail == buyerEmail &&
                        r.OrderStatus == true && 
                        r.IsAccepted == false && 
                        r.IsDelivered == false)
             .ToListAsync();

        public async Task<IEnumerable<RequestedProduct>> GetPendingProduct(string buyerEmail) =>
            await _applicationDbContext.RequestedProducts.Where(r => r.BuyerEmail == buyerEmail && r.OrderStatus == true && r.IsAccepted == true && r.IsDelivered == false).ToListAsync();

        public async Task<RequestedProduct> GetProductByProductIdAsync(string productId) => 
            _applicationDbContext.RequestedProducts.SingleOrDefault(p => p.Id == productId);

        public Task<IEnumerable<RequestedProduct>> GetRequestedProductsByBuyerIdAsync(string buyerId) =>
            throw new NotImplementedException();

        public Task<IEnumerable<RequestedProduct>> GetRequestedProductsByFarmerEmailAsync(string farmerEmail) =>
            throw new NotImplementedException();

        public async Task<IEnumerable<RequestedProduct>> GetRequestedProductsByFarmerIdAsync(string farmerId) =>
             await _applicationDbContext.RequestedProducts
                  .Where(r => r.Farmer.UserId == farmerId &&
                              r.OrderStatus == true && 
                              r.IsAccepted == false &&
                              r.IsDelivered == false && 
                               r.Haspaid == true)
                .ToListAsync();

        public RequestedProduct GetRequstedProductById(string requstedProductId) =>
            throw new NotImplementedException();

        public RequestedProduct UpdateRequestedProduct(RequestedProduct requestedProduct)
        {
          _applicationDbContext.RequestedProducts.Update(requestedProduct);
          _applicationDbContext.SaveChanges();
          return requestedProduct;
        }
    }
}