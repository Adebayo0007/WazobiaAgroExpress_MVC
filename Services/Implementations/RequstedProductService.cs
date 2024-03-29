using System.Security.Claims;
using Agro_Express.Dtos;
using Agro_Express.Dtos.RequestedProduct;
using Agro_Express.Email;
using Agro_Express.Models;
using Agro_Express.Repositories.Interfaces;
using Agro_Express.Services.Interfaces;
using static Agro_Express.Email.EmailDto;
using Microsoft.Extensions.Caching.Memory;

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
            private readonly IMemoryCache _memoryCache;

        public  RequestedProductService(IRequestedProductRepository requestedProductRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository,IFarmerRepository farmerRepository, IProductRepository productRepository, IEmailSender emailSender, IMemoryCache memoryCache)
        {
          _requestedProductRepository = requestedProductRepository;
          _httpContextAccessor = httpContextAccessor;
          _userRepository = userRepository;
          _farmerRepository = farmerRepository;
          _productRepository = productRepository;
          _emailSender = emailSender;
           _memoryCache = memoryCache;
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
             NotDelivered = false,
             FarmerEmail = farmer.User.Email,
             Haspaid = user.Haspaid

           };
           await _requestedProductRepository.CreateAsync(requestedProduct);
              string gender = null;
              string buyerGender = null;
              string pronoun = null;
               if(UserService.IsMale(farmer.User.Gender))
               {
                 gender="Mr";
               }
               else if(UserService.IsFeMale(farmer.User.Gender))
               {
                 gender="Mrs";
               }
               else
               {
                 gender = "Mr/Mrs";
               }

                if(UserService.IsMale(user.Gender))
               {
                 buyerGender="Mr";
                 pronoun = "him";
               }
               else if(UserService.IsFeMale(user.Gender))
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
            var products = await _requestedProductRepository.GetRequestedProductsByFarmerIdAsync(farmerId);
            var requestDto = products.Select(item  => RequestedProductDto(item)).ToList();
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

        public async Task NotDelivered(string productId)
        {
            var product = await _requestedProductRepository.GetProductByProductIdAsync(productId);
            var farmer = _userRepository.GetByEmailAsync(product.FarmerEmail);
            
            if(product.NotDelivered == false)
            {
              var email = new EmailRequestModel{
                SenderEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value,
                ReceiverEmail = "tijaniadebayoabdllahi@gmail.com",
                Subject = "User Complain",
                ReceiverName = "Admin",
                Message = $"I requested {product.ProductName} from  Mr/Mrs {farmer.Name} having this following information ( Number: {product.FarmerNumber} Email: {farmer.Email} User name: {farmer.UserName}),but the product is not delivered"

              };
               var response = await _emailSender.SendEmail(email);
                  product.NotDelivered = true;
                  _requestedProductRepository.UpdateRequestedProduct(product);
              
               if(response == true)
               {
                      var email1 = new EmailRequestModel{
                    ReceiverEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value,
                    Subject = "Complain Recieved",
                    ReceiverName = "Customer",
                    Message = $"Your complain has been recieved by Agro Express,we are SORRY for the inconveniences,we will connect withe farmer and get back to you as soon as possible.We are sorry once again"

                  };
                      await _emailSender.SendEmail(email1);
                  
               }
            }
            else{
                  await _requestedProductRepository.DeleteRequestedProduct(product);
                  var email2 = new EmailRequestModel{
                  ReceiverEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value,
                  Subject = "Agro Express say SORRY",
                  ReceiverName = "Customer",
                  Message = $"Agro Express say Sorry regarding the product that is not delivered to you,and we asure you that the farmer will be quried for that,Thank You for having your trust in us"

                };
               var response1 = await _emailSender.SendEmail(email2);
            }
        }

        public async Task<BaseResponse<OrderedAndPending>> OrderedAndPendingProduct(string buyerEmail)
        {
            var orderedProduct = await _requestedProductRepository.GetOrderedProduct(buyerEmail);
             var ordered = orderedProduct.Select(item => RequestedProductDto(item)).ToList();

            var pendingProduct = await _requestedProductRepository.GetPendingProduct(buyerEmail);

            var pending = pendingProduct.Select(item => RequestedProductDto(item)).ToList();
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
               
            var requestDto = RequestedProductDto(requestedProduct);
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

        private RequestedProductDto RequestedProductDto(RequestedProduct requestedProduct) =>
            new RequestedProductDto()
            {
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
                FarmerNumber = requestedProduct.FarmerNumber,
                NotDelivered = requestedProduct.NotDelivered

            };
    }
}