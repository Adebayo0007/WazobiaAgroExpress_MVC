using Agro_Express.Dtos;
using Agro_Express.Dtos.AllFarmers;
using Agro_Express.Dtos.Farmer;
using Agro_Express.Dtos.User;

namespace Agro_Express.Services.Interfaces
{
    public interface IFarmerService
    {
        Task<BaseResponse<FarmerDto>> CreateAsync(CreateFarmerRequestModel createFarmerModel);
        Task<BaseResponse<FarmerDto>> GetByIdAsync(string farmerId);
        Task<BaseResponse<FarmerDto>> GetByEmailAsync(string farmerEmail);
        Task<BaseResponse<IEnumerable<FarmerDto>>> GetAllAsync();
        Task<BaseResponse<ActiveAndNonActiveFarmer>> GetAllActiveAndNonActiveAsync();
         Task<BaseResponse<IEnumerable<FarmerDto>>> SearchFarmerByEmailOrUserName(string searchInput);
        Task<BaseResponse<FarmerDto>> UpdateAsync(UpdateFarmerRequestModel updateFarmerModel, string id);
        Task DeleteAsync(string farmerId);
        
    }
}