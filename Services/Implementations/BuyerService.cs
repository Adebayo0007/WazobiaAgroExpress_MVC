using Agro_Express.Dtos;
using Agro_Express.Dtos.AllBuyers;
using Agro_Express.Dtos.Buyer;
using Agro_Express.Dtos.User;
using Agro_Express.Email;
using Agro_Express.Models;
using Agro_Express.Repositories.Interfaces;
using Agro_Express.Services.Interfaces;
using static Agro_Express.Email.EmailDto;

namespace Agro_Express.Services.Implementations
{
    public class BuyerService : IBuyerService
    {
          private readonly IBuyerRepository _buyerRepository;
          private readonly IUserRepository _userRepository;
          private readonly IUserService _userService;
           private readonly IEmailSender _emailSender;
        public BuyerService(IBuyerRepository buyerRepository,IUserRepository userRepository, IUserService userService, IEmailSender emailSender)
        {
            _buyerRepository = buyerRepository;
            _userRepository = userRepository;
            _userService = userService;
            _emailSender = emailSender;
        }
        public async Task<BaseResponse<BuyerDto>> CreateAsync(CreateBuyerRequestModel createBuyerModel)
        {
           var response = await _emailSender.EmailValidaton(createBuyerModel.Email);
           if(response == false)
           {
              return new BaseResponse<BuyerDto>{
                IsSucess = false,
                Message = "your email is not valid,please check.",
            };
           }

            var address = new Address{
                    FullAddress = createBuyerModel.FullAddress,
                    LocalGovernment = createBuyerModel.LocalGovernment,
                    State = createBuyerModel.State
                  };
            var user = new User{
                  UserName = createBuyerModel.UserName.Trim(),
                  ProfilePicture = createBuyerModel.ProfilePicture,
                  Name = $"{createBuyerModel.FirstName} {createBuyerModel.LastName}",
                  PhoneNumber = createBuyerModel.PhoneNumber,
                  Address = address,
                  Gender = createBuyerModel.Gender,
                  Email = createBuyerModel.Email.Trim(),
                  Password = BCrypt.Net.BCrypt.HashPassword(createBuyerModel.Password),
                  Role = "Buyer",
                  IsActive = true,
                   IsRegistered = false,
                   Haspaid = false,
                   Due = true,
                  DateCreated = DateTime.Now

            };
            var userr = await _userRepository.CreateAsync(user);

            var buyer = new Buyer{
                UserId = userr.Id,
                User =  userr
            };
            var buyerModel = await _buyerRepository.CreateAsync(buyer);

                 string gender = null;
               if(UserService.IsMale(userr.Gender))
               {
                 gender="Mr";
               }
               else if(UserService.IsFeMale(userr.Gender))
               {
                 gender="Mrs";
               }
               else
               {
                 gender = "Mr/Mrs";
               }
               var email = new EmailRequestModel{
                 ReceiverEmail = userr.Email,
                 ReceiverName = userr.Name,
                 Subject = "Registration Confirmation",
                 Message = $"Thanks for signing up with Wazobia Agro Express {gender} {userr.Name} on {DateTime.Now.Date.ToString("dd/MM/yyyy")}.Your Registration need to be verified within today {DateTime.Now.Date.ToString("dd/MM/yyyy")} and {DateTime.Now.Date.AddDays(3).ToString("dd/MM/yyyy")} by the moderator before you can be authenticated to use the application for proper documentation and also you will recieve a mail immediately after verification.THANK YOU"
               };
               
             var mail =  await _emailSender.SendEmail(email);

             var buyerDto = BuyerDto(buyerModel);
            return new BaseResponse<BuyerDto>{
                IsSucess = true,
                Message = "Buyer Created successfully 😎",
                Data = buyerDto
            };
        }

        public async Task DeleteAsync(string buyerId)
        {
             var buyer = _userRepository.GetByIdAsync(buyerId);
            buyer.IsActive = buyer.IsActive.Equals(true) ? false : true;
            await _userRepository.Delete(buyer);
        }

        public async Task<BaseResponse<ActiveAndNonActiveBuyer>> GetAllActiveAndNonActiveAsync()
        {
            var nonActiveBuyers = await _buyerRepository.GetAllNonActiveAsync();
           if(nonActiveBuyers == null)
            {
                return new BaseResponse<ActiveAndNonActiveBuyer>
                {
                    Message = "No buyer Found 🙄",
                    IsSucess = false
                };  
            }
              var activebuyerDto = nonActiveBuyers.Select(a => BuyerDto(a)).ToList();
              var activeBuyers = await _buyerRepository.GetAllAsync();

           if(activeBuyers == null || nonActiveBuyers == null)
            {
                return new BaseResponse<ActiveAndNonActiveBuyer>
                {
                    Message = "No buyer Found 🙄",
                    IsSucess = false
                };  
            }
              var nonactivebuyerDto = activeBuyers.Select(a => BuyerDto(a)).ToList();

            var buyers = new ActiveAndNonActiveBuyer{
                ActiveBuyers = activebuyerDto,
                NonActiveBuyers = nonactivebuyerDto
            };

            return new BaseResponse<ActiveAndNonActiveBuyer>
            {
                Message = "List of Buyers 😎",
                IsSucess = true,
                Data = buyers
            };
        }

        public async Task<BaseResponse<IEnumerable<BuyerDto>>> GetAllAsync()
        {
             var buyers = await _buyerRepository.GetAllAsync();

           if(buyers == null)
            {
                return new BaseResponse<IEnumerable<BuyerDto>>
                {
                    Message = "No buyer Found 🙄",
                    IsSucess = false
                };  
            }
              var buyer = buyers.Select(a => BuyerDto(a)).ToList();
            return new BaseResponse<IEnumerable<BuyerDto>>
            {
                Message = "List of Buyers",
                IsSucess = true,
                Data = buyer
            };
        }

        public async Task<BaseResponse<BuyerDto>> GetByEmailAsync(string buyerEmail)
        {
             var buyer =  _buyerRepository.GetByEmailAsync(buyerEmail);
             if(buyer == null)
             {
                    return new BaseResponse<BuyerDto>
                {
                    Message = "Buyer not Found 🙄",
                    IsSucess = false
                };

             }
             BuyerDto buyerDto = null;
            if(buyer is not null)
            {
                  buyerDto = BuyerDto(buyer);
            }
            return new BaseResponse<BuyerDto>
            {
                Message = "Buyer Found successfully",
                IsSucess = true,
                Data = buyerDto
            };
        }

        public async Task<BaseResponse<BuyerDto>> GetByIdAsync(string buyerId)
        {
             var buyer =  _buyerRepository.GetByIdAsync(buyerId);
                if(buyer == null)
             {
                    return new BaseResponse<BuyerDto>
                {
                    Message = "Buyer not Found 🙄",
                    IsSucess = false
                };

             }
            var buyerDto = BuyerDto(buyer);
            return new BaseResponse<BuyerDto>
            {
                Message = "Buyer Found successfully",
                IsSucess = true,
                Data = buyerDto
            };
        }

        public async Task<BaseResponse<IEnumerable<BuyerDto>>> SearchBuyerByEmailOrUserName(string searchInput)
        {
            var buyers = await _buyerRepository.SearchBuyerByEmailOrUsername(searchInput);

           if(buyers == null)
            {
                return new BaseResponse<IEnumerable<BuyerDto>>
                {
                    Message = "No buyer Found 🙄",
                    IsSucess = false
                };  
            }
              var buyer = buyers.Select(a => BuyerDto(a)).ToList();
            return new BaseResponse<IEnumerable<BuyerDto>>
            {
                Message = "List of Buyers",
                IsSucess = true,
                Data = buyer
            };
        }

        public async Task<BaseResponse<BuyerDto>> UpdateAsync(UpdateBuyerRequestModel updateBuyerModel, string id)
        {
             var updateBuyer = new UpdateUserRequestModel{
                UserName = updateBuyerModel.UserName,
                ProfilePicture = updateBuyerModel.ProfilePicture ,
                Name = updateBuyerModel.Name,
                PhoneNumber  = updateBuyerModel.PhoneNumber,
                FullAddress = updateBuyerModel.FullAddress,
                LocalGovernment = updateBuyerModel.LocalGovernment,
                State  =updateBuyerModel.State,
                Gender = updateBuyerModel.Gender,
               Email = updateBuyerModel.Email,
               Password = (updateBuyerModel.Password) != null?BCrypt.Net.BCrypt.HashPassword(updateBuyerModel.Password): null,
            };
            var user = _userService.UpdateAsync(updateBuyer, id);
            if(user == null)
            {
                  return new BaseResponse<BuyerDto>{
                Message = "Buyer not updated,internal error 🙄",
                IsSucess = false
            };
            }
            var buyer = _buyerRepository.GetByEmailAsync(updateBuyerModel.Email);
            if(buyer == null)
            {
                    return new BaseResponse<BuyerDto>{
                    Message = "Buyer not updated,internal error 🙄",
                    IsSucess = false
                };
            }
             var buyerModel =  _buyerRepository.Update(buyer);

              var buyerDto =BuyerDto(buyerModel);

            return new BaseResponse<BuyerDto>{
                Message = "Buyer Updated successfully",
                IsSucess = true,
                Data = buyerDto
            };
        }

        private BuyerDto BuyerDto(Buyer buyer) => 
            new BuyerDto()
            {
                Id = buyer.User.Id,
                UserName = buyer.User.UserName,
                ProfilePicture = buyer.User.ProfilePicture,
                Name = buyer.User.Name,
                PhoneNumber = buyer.User.PhoneNumber,
                FullAddress = buyer.User.Address.FullAddress,
                LocalGovernment = buyer.User.Address.LocalGovernment,
                State = buyer.User.Address.State,
                Gender = buyer.User.Gender,
                Email = buyer.User.Email,
                Role = buyer.User.Role,
                IsActive = buyer.User.IsActive,
                DateCreated = buyer.User.DateCreated,
                DateModified = buyer.User.DateModified

            };
    }
}