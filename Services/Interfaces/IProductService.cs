using Agro_Express.Dtos;
using Agro_Express.Dtos.Product;

namespace Agro_Express.Services.Interfaces
{
    public interface IProductService
    {
        Task<BaseResponse<ProductDto>> CreateProductAsync(CreateProductRequestModel product);
        Task<BaseResponse<ProductDto>> GetProductById(string productId);
        Task<BaseResponse<IEnumerable<ProductDto>>> GetAllFarmProductAsync();
        Task<BaseResponse<IEnumerable<ProductDto>>> GetAllFarmProductByLocationAsync( string userEmail);
        Task<BaseResponse<IEnumerable<ProductDto>>> GetFarmerFarmProductsByEmailAsync(string email);
        Task<BaseResponse<IEnumerable<ProductDto>>> SearchProductsByProductNameOrFarmerUserNameOrFarmerEmail(string searchInput);
        Task<BaseResponse<ProductDto>> UpdateProduct(UpdateProductRequestModel product, string productId);
        Task DeleteProduct(string productId);
        Task ThumbUp(string productId);
        Task ThumbDown(string productId);
        Task DeleteExpiredProducts();
    }
}