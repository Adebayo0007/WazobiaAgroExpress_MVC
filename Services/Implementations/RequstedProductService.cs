using System.Security.Claims;
using Agro_Express.Dtos;
using Agro_Express.Dtos.RequestedProduct;
using Agro_Express.Email;
using Agro_Express.Models;
using Agro_Express.Repositories.Interfaces;
using Agro_Express.Services.Interfaces;
using static Agro_Express.Email.EmailDto;

namespace Agro_Express.Services.Implementations
{
    public class RequestedProductService : IRequestedProductService
    {
      
        private readonly IHttpContextAccessor _httpContextAccessor;
         private readonly IRequestedProductRepository _requestedProductRepository;
            private readonly IProductRepository _productRepository;
         private readonly IUserRepository _userRepository;
          private readonly IFarmerRepository _farmerRepository;
          
          private readonly IEmailSender _emailSender;

        public  RequestedProductService(IRequestedProductRepository requestedProductRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository,IFarmerRepository farmerRepository, IProductRepository productRepository, IEmailSender emailSender)
        {
          _requestedProductRepository = requestedProductRepository;
          _httpContextAccessor = httpContextAccessor;
          _userRepository = userRepository;
          _farmerRepository = farmerRepository;
          _productRepository = productRepository;
          _emailSender = emailSender;
        }
        public async Task<BaseResponse<RequestedProductDto>> CreateRequstedProductAsync(string productId)
        {
           var product = _productRepository.GetProductById(productId);
           var farmer = _farmerRepository.GetByEmailAsync(product.FarmerEmail);
           var userEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
           var user = _userRepository.GetByEmailAsync(userEmail);
           var requestedProduct = new  RequestedProduct{
             FarmerId = farmer.Id,
             BuyerEmail = user.Email,
             FarmerName = farmer.User.UserName,
             FarmerNumber = farmer.User.PhoneNumber,
             BuyerPhoneNumber = user.PhoneNumber,
             BuyerLocalGovernment = user.Address.LocalGovernment,
             ProductName = product.ProductName,
             OrderStatus = true,
             IsDelivered = false,
             IsAccepted = false,
             Haspaid = user.Haspaid

           };
           await _requestedProductRepository.CreateAsync(requestedProduct);
              string gender = null;
              string buyerGender = null;
              string pronoun = null;
               if(farmer.User.Gender ==  Enum.Gender.Male)
               {
                 gender="Mr";
               }
               else if(farmer.User.Gender==  Enum.Gender.Female)
               {
                 gender="Mrs";
               }
               else
               {
                 gender = "Mr/Mrs";
               }

                if(user.Gender == Enum.Gender.Male)
               {
                 buyerGender="Mr";
                 pronoun = "him";
               }
               else if(user.Gender== Enum.Gender.Female)
               {
                 buyerGender="Mrs";
                  pronoun = "her";
               }
               else
               {
                 buyerGender = "Mr/Mrs";
                 pronoun = "him/her";
               }

                   var email = new EmailRequestModel{
                 ReceiverEmail = farmer.User.Email,
                 ReceiverName = farmer.User.Name,
                 Subject = "Product Request",
                 Message = $"Hello {gender} {farmer.User.Name}!,An order has been made by {buyerGender} {user.Name} today {DateTime.Now.Date.ToString("dd/MM/yyyy")} requesting for {product.ProductName} on your portal,please kindly login to your portal to accept the offer.And also you can contact {pronoun} on {user.PhoneNumber},and don't forget to inform your client to click on the \'Delivered\' button on their portal after been accepted by you on your portal and delivered to {pronoun} successfully.Thank You"
               };
               await _emailSender.SendEmail(email);
            
           return new BaseResponse<RequestedProductDto>{
            IsSucess = true,
            Message = "Order Created successfully 😎"
           };
        }

        public async Task DeleteRequestedProduct(string productId)
        {
            var product = await _requestedProductRepository.GetProductByProductIdAsync(productId);
            await _requestedProductRepository.DeleteRequestedProduct(product);

               var email = new EmailRequestModel{
                 ReceiverEmail = product.BuyerEmail,
                 ReceiverName = product.BuyerEmail,
                 Subject = "Product Deleted",
                 Message = $"Hello!,the {product.ProductName} you ordered from Wazobia Agro Express have been deleted successfully,if you are still in need of the product or any other product,you can always go to the application and make your order!.For any complain or clearification contact 08087054632 or reply to this message.thank you"
               };
               await _emailSender.SendEmail(email);

        }

        public async Task<BaseResponse<IEnumerable<RequestedProductDto>>> MyRequests(string farmerId)
        {
             //farmerId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var products = await _requestedProductRepository.GetRequestedProductsByFarmerIdAsync(farmerId);
            var requestDto = products.Select(item  => new RequestedProductDto{
                Id = item.Id,
                FarmerId = item.FarmerId,
                BuyerId = item.FarmerId,
                BuyerEmail = item.BuyerEmail,
                BuyerPhoneNumber = item.BuyerPhoneNumber,
                BuyerLocalGovernment = item.BuyerLocalGovernment,
                ProductName = item.ProductName,
                OrderStatus = item.OrderStatus,
                IsAccepted = item.IsAccepted,
                IsDelivered = item.IsDelivered,
                FarmerName = item.FarmerName,
                FarmerNumber = item.FarmerNumber

            }).ToList();
            if(requestDto.Count() == 0)
            {
                    return new BaseResponse<IEnumerable<RequestedProductDto>>{
                    IsSucess = false,
                    Message ="No request yet 🙄",
                    Data = requestDto
                };
            }

            return new BaseResponse<IEnumerable<RequestedProductDto>>{
                IsSucess = true,
                Message ="These are your requets 😎",
                Data = requestDto
            };
        }

        public async Task<BaseResponse<OrderedAndPending>> OrderedAndPendingProduct(string buyerEmail)
        {
            var orderedProduct = await _requestedProductRepository.GetOrderedProduct(buyerEmail);
             var ordered = orderedProduct.Select(item => new RequestedProductDto{
                Id = item.Id,
                FarmerId = item.FarmerId,
                BuyerId = item.FarmerId,
                BuyerEmail = item.BuyerEmail,
                BuyerPhoneNumber = item.BuyerPhoneNumber,
                BuyerLocalGovernment = item.BuyerLocalGovernment,
                ProductName = item.ProductName,
                OrderStatus = item.OrderStatus,
                IsAccepted = item.IsAccepted,
                IsDelivered = item.IsDelivered,
                FarmerName = item.FarmerName,
                FarmerNumber = item.FarmerNumber
            });

            var pendingProduct = await _requestedProductRepository.GetPendingProduct(buyerEmail);

            var pending = pendingProduct.Select(item => new RequestedProductDto{
                Id = item.Id,
                FarmerId = item.FarmerId,
                BuyerId = item.FarmerId,
                BuyerEmail = item.BuyerEmail,
                BuyerPhoneNumber = item.BuyerPhoneNumber,
                BuyerLocalGovernment = item.BuyerLocalGovernment,
                ProductName = item.ProductName,
                OrderStatus = item.OrderStatus,
                IsAccepted = item.IsAccepted,
                IsDelivered = item.IsDelivered,
                FarmerName = item.FarmerName,
                FarmerNumber = item.FarmerNumber
            });
            var orderedAndPending = new OrderedAndPending{
                OrderedProduct = ordered,
                PendingProduct = pending
            };
            return new BaseResponse<OrderedAndPending>{
                IsSucess = true,
                Message = "Product ordered succesfully",
                Data = orderedAndPending
            };
        }

        public async Task<BaseResponse<RequestedProductDto>> ProductAccepted(string productId)
        {
            var requestedProduct =  await _requestedProductRepository.GetProductByProductIdAsync(productId);
            requestedProduct.IsAccepted = true;
            _requestedProductRepository.UpdateRequestedProduct(requestedProduct);

              var email = new EmailRequestModel{
                 ReceiverEmail = requestedProduct.BuyerEmail,
                 ReceiverName = requestedProduct.BuyerEmail,
                 Subject = "Product Accepted",
                 Message =$"Hello!,the {requestedProduct.ProductName} you ordered from Wazobia Agro Express have been accepted by the farmer,the farmer will contact you in short time,stay tune!.For any complain or clearification contact 08087054632 or reply to this message"
               };
               await _emailSender.SendEmail(email);
               
            var requestDto = new RequestedProductDto{
                Id = requestedProduct.Id,
                FarmerId = requestedProduct.FarmerId,
                BuyerId = requestedProduct.FarmerId,
                BuyerEmail = requestedProduct.BuyerEmail,
                BuyerPhoneNumber = requestedProduct.BuyerPhoneNumber,
                BuyerLocalGovernment = requestedProduct.BuyerLocalGovernment,
                ProductName = requestedProduct.ProductName,
                OrderStatus = requestedProduct.OrderStatus,
                IsAccepted = requestedProduct.IsAccepted,
                IsDelivered = requestedProduct.IsDelivered,
                FarmerName = requestedProduct.FarmerName,
                FarmerNumber = requestedProduct.FarmerNumber
            };
            return new BaseResponse<RequestedProductDto>{
                IsSucess = true,
                Message = "Request accepted successfully",
                Data = requestDto
            };
        }

        public async Task  ProductDelivered(string productId)
        {
           var requestedProduct =  await _requestedProductRepository.GetProductByProductIdAsync(productId);
           var id =  _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var farmer = _farmerRepository.GetByIdAsync(requestedProduct.FarmerId);
        if(farmer.UserId != id)
        {
           farmer.Ranking++;
           _farmerRepository.Update(farmer);
           
               var email = new EmailRequestModel{
                 ReceiverEmail = requestedProduct.BuyerEmail,
                 ReceiverName = requestedProduct.BuyerEmail,
                 Subject = "Successful Marketing",
                 Message = $"Wazobia Agro Express is saying Thank you for using our Application as market place, we look forward to see you next time on our apllication 😎.Thank you"
               };
               await _emailSender.SendEmail(email);
               
           
        }
            await _requestedProductRepository.DeleteRequestedProduct(requestedProduct);
        }
    }
}