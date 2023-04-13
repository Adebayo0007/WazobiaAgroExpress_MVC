using Agro_Express.Dtos;
using Agro_Express.Dtos.AllBuyers;
using Agro_Express.Dtos.Buyer;

namespace Agro_Express.Services.Interfaces
{
    public interface IBuyerService
    {
        Task<BaseResponse<BuyerDto>> CreateAsync(CreateBuyerRequestModel createBuyerModel);
        Task<BaseResponse<BuyerDto>> GetByIdAsync(string buyerId);
        Task<BaseResponse<BuyerDto>> GetByEmailAsync(string buyerEmail);
        Task<BaseResponse<IEnumerable<BuyerDto>>> GetAllAsync();
         Task<BaseResponse<ActiveAndNonActiveBuyer>> GetAllActiveAndNonActiveAsync();
        Task<BaseResponse<IEnumerable<BuyerDto>>> SearchBuyerByEmailOrUserName(string searchInput);
        Task<BaseResponse<BuyerDto>> UpdateAsync(UpdateBuyerRequestModel updateBuyerModel, string id);
        Task DeleteAsync(string buyerId);
    }
}