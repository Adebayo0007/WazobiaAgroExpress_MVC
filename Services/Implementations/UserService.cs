using System.Security.Claims;
using Agro_Express.Dtos;
using Agro_Express.Dtos.User;
using Agro_Express.Email;
using Agro_Express.Enum;
using Agro_Express.Models;
using Agro_Express.Repositories.Interfaces;
using Agro_Express.Services.Interfaces;
using static Agro_Express.Email.EmailDto;
using Microsoft.Extensions.Caching.Memory;

namespace Agro_Express.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
          private readonly IHttpContextAccessor _httpContextAccessor;
          private readonly IEmailSender _emailSender;
           private readonly IMemoryCache _memoryCache;
        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender, IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
             _memoryCache = memoryCache;
            
        }
 
        public async Task DeleteAsync(string userId)
        {
              if (!_memoryCache.TryGetValue($"User_With_Id_{userId}", out User user))
            {
                 user = _userRepository.GetByIdAsync(userId);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3) // Cache for 3 minutes
                };
                 _memoryCache.Set($"User_With_Id_{userId}", user, cacheEntryOptions);

            }
            user.IsActive = user.IsActive == true? false: true;
             await _userRepository.Delete(user);
        }

        public async Task<BaseResponse<IEnumerable<UserDto>>> GetAllAsync()
        {
           
            var users = await _userRepository.GetAllAsync();

           if(users == null)
            {
                return new BaseResponse<IEnumerable<UserDto>>
                {
                    Message = "No user Found 🙄",
                    IsSucess = false,
                
                };  
            }
              var user = users.Select(u => UserDto(u)).ToList();
            return new BaseResponse<IEnumerable<UserDto>>
            {
                Message = "List of Users 😎",
                IsSucess = true,
                Data = user
            };
        }

        public async Task<BaseResponse<UserDto>> GetByEmailAsync(string userEmail)
        {
               if (!_memoryCache.TryGetValue($"User_With_Email_{userEmail}", out User user))
            {
                 user = _userRepository.GetByEmailAsync(userEmail);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3) // Cache for 3 minutes
                };
                 _memoryCache.Set($"User_With_Email_{userEmail}", user, cacheEntryOptions);

            }
           
            if(user == null)
            {
                    return new BaseResponse<UserDto>
                {
                    Message = "User not Found 🙄",
                    IsSucess = false
                };
            }
            UserDto userDto = null;
            if(user is not null)
            {
                userDto = UserDto(user);
            }
            return new BaseResponse<UserDto>
            {
                Message = "User Found successfully 😎",
                IsSucess = true,
                Data = userDto
            };
        }
        public async Task<BaseResponse<UserDto>> Login(LogInRequestModel logInRequestMode)
        {
            var email = await _userRepository.ExistByEmailAsync(logInRequestMode.Email);
              if (!_memoryCache.TryGetValue($"User_With_Email_{logInRequestMode.Email}", out User user))
            {
                 user =  _userRepository.GetByEmailAsync(logInRequestMode.Email);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Cache for 5 minutes
                };
                 _memoryCache.Set($"User_With_Email_{logInRequestMode.Email}", user, cacheEntryOptions);

            }
         
                 if(user.IsRegistered == false)
                {
                      return new BaseResponse<UserDto>
                    {
                        Message = "Your Registeration is yet to be verified 🙄",
                        IsSucess = false
                    };
                }

                   if(user.IsActive == false)
                {
                      return new BaseResponse<UserDto>
                    {
                        Message = "You are not an active user 🙄",
                        IsSucess = false
                    };
                }

                if(user.Due == false)
                {
                      return new BaseResponse<UserDto>
                    {
                        Message = "Due",
                        IsSucess = false
                    };
                }


               //for user using temporary password
               var passwordCheck = logInRequestMode.Password.Split("0");
               if(passwordCheck[0] == "$2b$1")
               {
                    if(user.Password == logInRequestMode.Password && user.Email == logInRequestMode.Email)
                    {
                            UserDto userDto1 = null;

                        if(user is not null)
                        {
                            userDto1 = UserDto(user);
                        }

                        return new BaseResponse<UserDto>
                        {
                            Message = "Login successfully 😎",
                            IsSucess = true,
                            Data = userDto1
                        };
                    }
                     else
                        {
                            return new BaseResponse<UserDto>
                            {
                                Message = "Invalid email/password 🙄",
                                IsSucess = false
                            };
                        }
               
               }


                 //for user using their password
                var password = BCrypt.Net.BCrypt.Verify(logInRequestMode.Password, user.Password);

                if(email == false || password == false)
                {
                       return new BaseResponse<UserDto>
                    {
                        Message = "Invalid email/password 🙄",
                        IsSucess = false
                    };
                }

             
                   UserDto userDto = null;

             if(user is not null)
            {
                  userDto = UserDto(user);
            }

            return new BaseResponse<UserDto>
            {
                Message = "Login successfully 😎",
                IsSucess = true,
                Data = userDto
            }; 

        }

        public async Task<BaseResponse<UserDto>> GetByIdAsync(string userId)
        {
              if (!_memoryCache.TryGetValue($"User_With_Id_{userId}", out User user))
            {
                 user = _userRepository.GetByIdAsync(userId);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3) // Cache for 3 minutes
                };
                 _memoryCache.Set($"User_With_Id_{userId}", user, cacheEntryOptions);

            }
            
            if(user == null)
            {
                    return new BaseResponse<UserDto>
                {
                    Message = "User not Found 🙄",
                    IsSucess = false
                };
            }
            var userDto = UserDto(user);
            return new BaseResponse<UserDto>
            {
                Message = "User Found successfully",
                IsSucess = true,
                Data = userDto
            };
        }

        public BaseResponse<UserDto> UpdateAsync(UpdateUserRequestModel updateUserModel, string userId)
        {
              if (!_memoryCache.TryGetValue($"User_With_Id_{userId}", out User user))
            {
                 user = _userRepository.GetByIdAsync(userId);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3) // Cache for 3 minutes
                };
                 _memoryCache.Set($"User_With_Id_{userId}", user, cacheEntryOptions);

            }
            
            if(user == null)
            {
                 return new BaseResponse<UserDto>
                {
                    Message = "User not updated,internal error 🙄",
                    IsSucess = false
                };
            }
            user.UserName = updateUserModel.UserName ??  user.UserName;
            user.ProfilePicture = updateUserModel.ProfilePicture ?? user.ProfilePicture;
            user.Name = updateUserModel.Name ?? user.Name;
            user.PhoneNumber  = updateUserModel.PhoneNumber ?? user.PhoneNumber;
            user.Address.FullAddress = updateUserModel.FullAddress ?? user.Address.FullAddress;
            user.Address.LocalGovernment = (int)updateUserModel.LocalGovernment  != 0?updateUserModel.LocalGovernment :user.Address.LocalGovernment;
            user.Address.State = (int)updateUserModel.State  != 0 ?updateUserModel.State :user.Address.State;
            user.Gender = (int)updateUserModel.Gender  != 0 ?updateUserModel.Gender :user.Gender;
            user.Email = updateUserModel.Email ?? user.Email;
            user.Password = updateUserModel.Password ?? user.Password;
            user.DateModified = DateTime.Now;
            _userRepository.Update(user);

                var email = new EmailRequestModel{
                 ReceiverEmail = user.Email,
                 ReceiverName = user.Name,
                 Subject = "Profile Updated",
                 Message = $"Hello {user.Name}!,Your Profile on Wazobia Agro Express have been updated successfully.For any complain or clearification contact 08087054632 or reply to this message"
               };
                 _emailSender.SendEmail(email);
            
            return new BaseResponse<UserDto>
            {
                Message = "User Updated successfully",
                IsSucess = true
            };

        }
        public async Task<bool> ExistByEmailAsync(string userEmail) =>
           await _userRepository.ExistByEmailAsync(userEmail);

        public async Task<BaseResponse<IEnumerable<UserDto>>> SearchUserByEmailOrUserName(string searchInput)
        {
             var users = await _userRepository.SearchUserByEmailOrUsername(searchInput);

           if(users == null)
            {
                return new BaseResponse<IEnumerable<UserDto>>
                {
                    Message = "No user Found 🙄",
                    IsSucess = false,
                
                };  
            }
              var user = users.Select(a => UserDto(a)).ToList();
            return new BaseResponse<IEnumerable<UserDto>>
            {
                Message = "List of Users 😎",
                IsSucess = true,
                Data = user
            };
        }

        public async Task<BaseResponse<IEnumerable<UserDto>>> PendingRegistration()
        {
               var users = await _userRepository.PendingRegistration();

           if(users == null)
            {
                return new BaseResponse<IEnumerable<UserDto>>
                {
                    Message = "No user Found 🙄",
                    IsSucess = false,
                
                };  
            }
              var user = users.Select(a => UserDto(a)).ToList();
            return new BaseResponse<IEnumerable<UserDto>>
            {
                Message = "List of Users 😎",
                IsSucess = true,
                Data = user
            };
        }

        public BaseResponse<UserDto> VerifyUser(string userEmail)
        {
             if (!_memoryCache.TryGetValue($"User_With_Email_{userEmail}", out User user))
            {
                 user =  _userRepository.GetByEmailAsync(userEmail);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Cache for 5 minutes
                };
                 _memoryCache.Set($"User_With_Email_{userEmail}", user, cacheEntryOptions);

            }
           
           user.IsRegistered = true;
           _userRepository.Update(user);

            string gender = null;
              if(IsMale(user.Gender))
               {
                 gender="Mr";
               }
               else if(IsFeMale(user.Gender))
               {
                 gender="Mrs";
               }
               else
               {
                 gender = "Mr/Mrs";
               }

                var email = new EmailRequestModel{
                 ReceiverEmail = user.Email,
                 ReceiverName = user.Name,
                 Subject = "Successful verification",
                 Message = $"Congratulations🎁! {gender} {user.Name},my Name is Mr Adebayo (The moderator of Wazobia Agro Express),I am happy to tell you that your Wazobia Agro Express Account have been verified today on {DateTime.Now.Date.ToString("dd/MM/yyyy")}.You can now login with the submitted details successfully.You can also contact the moderator for any confirmation or help on 08087054632.YOU are welcome to Wazobia Agro Express {gender} {user.Name}👍"
               };
                 _emailSender.SendEmail(email);

            return new BaseResponse<UserDto>
            {
                Message = "User verified successfully 😎",
                IsSucess = true
            };
        }

        public Task UpdatingToHasPaid(string email)
        {
           email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
             if (!_memoryCache.TryGetValue($"User_With_Email_{email}", out User user))
            {
                 user =  _userRepository.GetByEmailAsync(email);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Cache for 5 minutes
                };
                 _memoryCache.Set($"User_With_Email_{email}", user, cacheEntryOptions);

            }
           
           user.Haspaid = true;
           _userRepository.Update(user);
           return null;
        }

        public async Task<bool> ForgottenPassword(string email)
        {
             if (!_memoryCache.TryGetValue($"User_With_Email_{email}", out User user))
            {
                 user =  _userRepository.GetByEmailAsync(email);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Cache for 5 minutes
                };
                 _memoryCache.Set($"User_With_Email_{email}", user, cacheEntryOptions);

            }
          
             var emailRequestModel = new EmailRequestModel{
                 ReceiverEmail = email,
                 ReceiverName = email,
                 Subject = "Password Reset",
                 Message = $"Copy your temporary password details and use it to login,then you can reset the password after logging in; {user.Password}  "
               };
                 var mail = await _emailSender.SendEmail(emailRequestModel);
                 if(mail == true)
                 {
                    return true;
                 }
                 return false;
        }

        private UserDto UserDto(User user) => 
            new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                ProfilePicture = user.ProfilePicture,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                FullAddress = user.Address.FullAddress,
                LocalGovernment = user.Address.LocalGovernment,
                State = user.Address.State,
                Gender = user.Gender,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                DateCreated = user.DateCreated,
                DateModified = user.DateModified
            };
            public static bool IsMale(Gender gender)
            {
                if (!gender.Equals(null))
                {
                    if (gender.Equals(Enum.Gender.Male))
                        return true;
                }
                return false;
            }
            public static bool IsFeMale(Gender gender)
            {
                if (!gender.Equals(null))
                {
                    if (gender.Equals(Enum.Gender.Male))
                        return true;
                }
                return false;
            }
    }
}