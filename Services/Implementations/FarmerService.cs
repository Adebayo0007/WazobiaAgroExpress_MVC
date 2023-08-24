using Agro_Express.Dtos;
using Agro_Express.Dtos.AllFarmers;
using Agro_Express.Dtos.Farmer;
using Agro_Express.Dtos.User;
using Agro_Express.Email;
using Agro_Express.Models;
using Agro_Express.Repositories.Interfaces;
using Agro_Express.Services.Interfaces;
using static Agro_Express.Email.EmailDto;
using Microsoft.Extensions.Caching.Memory;

namespace Agro_Express.Services.Implementations
{
    public class FarmerService : IFarmerService
    {
         private readonly IFarmerRepository _farmerRepository;
          private readonly IUserRepository _userRepository;
          private readonly IUserService _userService;
             private readonly IEmailSender _emailSender;
               private readonly IHttpContextAccessor _httpContextAccessor;
               private readonly IMemoryCache _memoryCache;
        public FarmerService(IFarmerRepository farmerRepository,IUserRepository userRepository, IUserService userService,  IEmailSender emailSender, IHttpContextAccessor httpContextAccessor,IMemoryCache memoryCache )
        {
            _farmerRepository = farmerRepository;
            _userRepository = userRepository;
            _userService = userService;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
             _memoryCache = memoryCache;
            
        }
        public async Task<BaseResponse<FarmerDto>> CreateAsync(CreateFarmerRequestModel createFarmerModel)
        {

               var response = await _emailSender.EmailValidaton(createFarmerModel.Email);
           if(response == false)
           {
              return new BaseResponse<FarmerDto>{
                IsSucess = false,
                Message = "your email is not valid,please check.",
            };
           }
             var address = new Address{
                    FullAddress = createFarmerModel.FullAddress,
                    LocalGovernment = createFarmerModel.LocalGovernment,
                    State = createFarmerModel.State
                  };
            var user = new User{
                  UserName = createFarmerModel.UserName.Trim(),
                  ProfilePicture = createFarmerModel.ProfilePicture,
                  Name = $"{createFarmerModel.FirstName} {createFarmerModel.LastName}",
                  PhoneNumber = createFarmerModel.PhoneNumber,
                  Address = address,
                  Gender = createFarmerModel.Gender,
                  Email = createFarmerModel.Email.Trim(),
                  Password = BCrypt.Net.BCrypt.HashPassword(createFarmerModel.Password),
                  Role = "Farmer",
                  IsActive = true,
                  IsRegistered = false,
                  Haspaid = false,
                  Due = true,
                  DateCreated = DateTime.Now

            };
            var userr = await _userRepository.CreateAsync(user);

            var farmer = new Farmer{
                UserId = userr.Id,
                User =  userr
            };
            var farmerModel = await _farmerRepository.CreateAsync(farmer);

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

             var farmerDto = FarmerDto(farmerModel);
            return new BaseResponse<FarmerDto>{
                IsSucess = true,
                Message = "Farmer Created successfully 😎",
                Data = farmerDto
            };
        }

        public async Task DeleteAsync(string farmerId)
        {
             if (!_memoryCache.TryGetValue($"Farmer_With_Id_{farmerId}", out User farmer))
            {
                 farmer = _userRepository.GetByIdAsync(farmerId);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3) // Cache for 3 minutes
                };
                 _memoryCache.Set($"Farmer_With_Id_{farmerId}", farmer, cacheEntryOptions);

            }
         
            farmer.IsActive = farmer.IsActive.Equals(true) ? false : true;
              await _userRepository.Delete(farmer);
        }

        public async Task<BaseResponse<IEnumerable<FarmerDto>>> GetAllAsync()
        {
            var farmers = await _farmerRepository.GetAllAsync();

           if(farmers == null)
            {
                return new BaseResponse<IEnumerable<FarmerDto>>
                {
                    Message = "No farmer Found 🙄",
                    IsSucess = false
                };  
            }
              var farmer = farmers.Select(a => FarmerDto(a)).ToList();
            return new BaseResponse<IEnumerable<FarmerDto>>
            {
                Message = "List of Farmers 😎",
                IsSucess = true,
                Data = farmer
            };
        }

        public  async Task<BaseResponse<ActiveAndNonActiveFarmer>> GetAllActiveAndNonActiveAsync()
        {
             var nonActiveFarmers = await _farmerRepository.GetAllNonActiveAsync();

           if(nonActiveFarmers == null)
            {
                return new BaseResponse<ActiveAndNonActiveFarmer>
                {
                    Message = "No farmer Found 🙄",
                    IsSucess = false
                };  
            }
              var farmer = nonActiveFarmers.Select(a => FarmerDto(a)).ToList();
              var ActiveFarmers = await _farmerRepository.GetAllAsync();

           if(ActiveFarmers == null)
            {
                return new BaseResponse<ActiveAndNonActiveFarmer>
                {
                    Message = "No farmer Found 🙄",
                    IsSucess = false
                };  
            }
              var farmerr = ActiveFarmers.Select(a => FarmerDto(a)).ToList();

            var farmers = new ActiveAndNonActiveFarmer{
                ActiveFarmers = farmer,
                NonActiveFarmers = farmerr
            };

            return new BaseResponse<ActiveAndNonActiveFarmer>
            {
                Message = "List of Farmers 😎",
                IsSucess = true,
                Data = farmers
            };
        }

        public async Task<BaseResponse<FarmerDto>> GetByEmailAsync(string farmerEmail)
        {
              if (!_memoryCache.TryGetValue($"Farmer_With_Email_{farmerEmail}", out Farmer farmer))
            {
                 farmer = _farmerRepository.GetByEmailAsync(farmerEmail);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3) // Cache for 3 minutes
                };
                 _memoryCache.Set($"Farmer_With_Email_{farmerEmail}", farmer, cacheEntryOptions);

            }
          
             if(farmer == null)
             {
                        return new BaseResponse<FarmerDto>
                    {
                        Message = "Farmer not Found 🙄",
                        IsSucess = false
                    };
             }
            FarmerDto farmerDto = null;
            if(farmer is not null)
            {
                  farmerDto = FarmerDto(farmer);
            }
            return new BaseResponse<FarmerDto>
            {
                Message = "Farmer Found successfully 😎",
                IsSucess = true,
                Data = farmerDto
            };
        }

        public async Task<BaseResponse<FarmerDto>> GetByIdAsync(string farmerId)
        {
             if (!_memoryCache.TryGetValue($"Farmer_With_Id_{farmerId}", out Farmer farmer))
            {
                 farmer = _farmerRepository.GetByIdAsync(farmerId);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3) // Cache for 3 minutes
                };
                 _memoryCache.Set($"Farmer_With_Id_{farmerId}", farmer, cacheEntryOptions);

            }
             
              if(farmer == null)
             {
                        return new BaseResponse<FarmerDto>
                    {
                        Message = "Farmer not Found 🙄",
                        IsSucess = false
                    };
             }
            var farmerDto = FarmerDto(farmer);
            return new BaseResponse<FarmerDto>
            {
                Message = "Farmer Found successfully",
                IsSucess = true,
                Data = farmerDto
            };
        }

        public async Task<BaseResponse<FarmerDto>> UpdateAsync(UpdateFarmerRequestModel updateFarmerModel, string id)
        {

            var updateFarmer = new UpdateUserRequestModel{
                UserName = updateFarmerModel.UserName,
                ProfilePicture = updateFarmerModel.ProfilePicture ,
                Name = updateFarmerModel.Name,
                PhoneNumber  = updateFarmerModel.PhoneNumber,
                FullAddress = updateFarmerModel.FullAddress,
                LocalGovernment = updateFarmerModel.LocalGovernment,
                State  =updateFarmerModel.State,
                Gender = updateFarmerModel.Gender,
               Email = updateFarmerModel.Email,
               Password = (updateFarmerModel.Password) != null?BCrypt.Net.BCrypt.HashPassword(updateFarmerModel.Password): null,
            };
            var user = _userService.UpdateAsync(updateFarmer, id);
              if(user == null)
            {
                  return new BaseResponse<FarmerDto>{
                Message = "farmer not updated,internal error 🙄",
                IsSucess = false
            };
            }
            var farmer = _farmerRepository.GetByEmailAsync(updateFarmerModel.Email);
            if(farmer == null)
            {
                  return new BaseResponse<FarmerDto>{
                Message = "farmer not updated,internal error 🙄",
                IsSucess = false
            };
            }
              var farmerModel = _farmerRepository.Update(farmer);

              var farmerDto = FarmerDto(farmerModel);

            return new BaseResponse<FarmerDto>{
                Message = "Farmer Updated successfully",
                IsSucess = true,
                Data = farmerDto
            };
        }

        public async Task<BaseResponse<IEnumerable<FarmerDto>>> SearchFarmerByEmailOrUserName(string searchInput)
        {
             var farmers = await _farmerRepository.SearchFarmerByEmailOrUsername(searchInput);

           if(farmers == null)
            {
                return new BaseResponse<IEnumerable<FarmerDto>>
                {
                    Message = "No farmer Found 🙄",
                    IsSucess = false
                };  
            }
              var farmer = farmers.Select(a => FarmerDto(a)).ToList();
            return new BaseResponse<IEnumerable<FarmerDto>>
            {
                Message = "List of Farmers 😎",
                IsSucess = true,
                Data = farmer
            };
        }

        public async Task FarmerMonthlyDueUpdate()
        {
            if(DateTime.Now.Date.AddDays(-1).Month == DateTime.Now.Date.Month)
            {
               await _farmerRepository.FarmerMonthlyDueUpdate();
            }
        }

        public Task UpdateToHasPaidDue(string userEmail)
        {
             if (!_memoryCache.TryGetValue($"Farmer_With_Email_{userEmail}", out User user))
            {
                 user = _userRepository.GetByEmailAsync(userEmail);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3) // Cache for 3 minutes
                };
                 _memoryCache.Set($"Farmer_With_Email_{userEmail}", user, cacheEntryOptions);

            }
          user.Due = true;
          _userRepository.Update(user);
           return null;
        
        }

        private FarmerDto FarmerDto(Farmer farmer) => 
            new FarmerDto()
            {
                UserName = farmer.User.UserName,
                ProfilePicture = farmer.User.ProfilePicture,
                Name = farmer.User.Name,
                PhoneNumber = farmer.User.PhoneNumber,
                FullAddress = farmer.User.Address.FullAddress,
                LocalGovernment = farmer.User.Address.LocalGovernment,
                State = farmer.User.Address.State,
                Gender = farmer.User.Gender,
                Email = farmer.User.Email,
                Password = farmer.User.Password,
                Role = farmer.User.Role,
                IsActive = farmer.User.IsActive,
                DateCreated = farmer.User.DateCreated,
                DateModified = farmer.User.DateModified,
                Ranking = farmer.Ranking
            };
    }
}