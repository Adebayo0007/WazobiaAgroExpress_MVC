using Agro_Express.Dtos;
using Agro_Express.Dtos.RequestedProduct;

namespace Agro_Express.Services.Interfaces
{
    public interface IRequestedProductService
    {
        Task<BaseResponse<RequestedProductDto>> CreateRequstedProductAsync(string productId);   
        Task<BaseResponse<OrderedAndPending>> OrderedAndPendingProduct(string buyerEmail);  
        Task DeleteRequestedProduct(string productId); 
        Task ProductDelivered(string productId);
        Task<BaseResponse<RequestedProductDto>> ProductAccepted(string productId);
         Task<BaseResponse<IEnumerable<RequestedProductDto>>> MyRequests(string farmerId);
    }

}