using Agro_Express.Dtos;
using Agro_Express.Dtos.User;
using Agro_Express.Repositories.Interfaces;
using Agro_Express.Services.Interfaces;

namespace Agro_Express.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            
        }
 
        public async Task DeleteAsync(string userId)
        {
            var user = _userRepository.GetByIdAsync(userId);
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
              var user = users.Select(a => new UserDto{
                  Id = a.Id,
                  UserName = a.UserName,
                  ProfilePicture = a.ProfilePicture,
                  Name = a.Name,
                  PhoneNumber = a.PhoneNumber,
                  FullAddress = a.Address.FullAddress ,
                  LocalGovernment = a.Address.LocalGovernment,
                  State = a.Address.State,
                  Gender = a.Gender,
                  Email = a.Email,
                  Password = a.Password,
                  Role = a.Role,
                  IsActive = a.IsActive,
                  DateCreated = a.DateCreated,
                  DateModified = a.DateModified
            }).ToList();
            return new BaseResponse<IEnumerable<UserDto>>
            {
                Message = "List of Users 😎",
                IsSucess = true,
                Data = user
            };
        }

        public async Task<BaseResponse<UserDto>> GetByEmailAsync(string userEmail)
        {
            var user =  _userRepository.GetByEmailAsync(userEmail);
            if(user == null)
            {
                    return new BaseResponse<UserDto>
                {
                    Message = "User not Found 🙄",
                    IsSucess = false
                };
            }
            UserDto userDto = new UserDto();
            if(user is not null)
            {
                  userDto.Id = user.Id;
                  userDto.UserName = user.UserName;
                  userDto. ProfilePicture =  user.ProfilePicture;
                  userDto.Name =  user.Name;
                  userDto.PhoneNumber =  user.PhoneNumber;
                  userDto.FullAddress =  user.Address.FullAddress ;
                  userDto.LocalGovernment =  user.Address.LocalGovernment;
                  userDto.State =  user.Address.State;
                  userDto.Gender = user.Gender;
                  userDto.Email = user.Email;
                //   userDto.Password = user.Password;
                  userDto.Role = user.Role;
                  userDto.IsActive = user.IsActive;
                  userDto.DateCreated = user.DateCreated;
                  userDto.DateModified = user.DateModified;
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
            var email =await _userRepository.ExistByEmailAsync(logInRequestMode.Email);
         
               var user = _userRepository.GetByEmailAsync(logInRequestMode.Email);

                var password = BCrypt.Net.BCrypt.Verify(logInRequestMode.Password, user.Password);
                  if(user.IsRegistered == false)
                {
                      return new BaseResponse<UserDto>
                    {
                        Message = "Your Registeration is yet to be verified 🙄",
                        IsSucess = false
                    };
                }

                if(email == false || password == false)
                {
                       return new BaseResponse<UserDto>
                    {
                        Message = "Invalid email/password 🙄",
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
                   UserDto userDto = new UserDto();

             if(user is not null)
            {
                  userDto.Id = user.Id;
                  userDto.UserName = user.UserName;
                  userDto. ProfilePicture =  user.ProfilePicture;
                  userDto.Name =  user.Name;
                  userDto.PhoneNumber =  user.PhoneNumber;
                  userDto.FullAddress =  user.Address.FullAddress ;
                  userDto.LocalGovernment =  user.Address.LocalGovernment;
                  userDto.State =  user.Address.State;
                  userDto.Gender = user.Gender;
                  userDto.Email = user.Email;
                //   userDto.Password = user.Password;
                  userDto.Role = user.Role;
                  userDto.IsActive = user.IsActive;
                  userDto.DateCreated = user.DateCreated;
                  userDto.DateModified = user.DateModified;
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
            var user = _userRepository.GetByIdAsync(userId);
            if(user == null)
            {
                    return new BaseResponse<UserDto>
                {
                    Message = "User not Found 🙄",
                    IsSucess = false
                };
            }
            var userDto = new UserDto{
                     Id = user.Id,
                     UserName = user.UserName,
                     ProfilePicture =  user.ProfilePicture,
                     Name =  user.Name,
                     PhoneNumber =  user.PhoneNumber,
                     FullAddress =  user.Address.FullAddress ,
                     LocalGovernment =  user.Address.LocalGovernment,
                     State =  user.Address.State,
                     Gender = user.Gender,
                     Email = user.Email,
                     Password = user.Password,
                     Role = user.Role,
                     IsActive = user.IsActive,
                     DateCreated = user.DateCreated,
                     DateModified = user.DateModified

            };
            return new BaseResponse<UserDto>
            {
                Message = "User Found successfully",
                IsSucess = true,
                Data = userDto
            };
        }

        public BaseResponse<UserDto> UpdateAsync(UpdateUserRequestModel updateUserModel, string userId)
        {
            var user =  _userRepository.GetByIdAsync(userId);
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
            return new BaseResponse<UserDto>
            {
                Message = "User Updated successfully",
                IsSucess = true
            };

        }
        public async Task<bool> ExistByEmailAsync(string userEmail)
        {
          return await _userRepository.ExistByEmailAsync(userEmail);
        }

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
              var user = users.Select(a => new UserDto{
                  Id = a.Id,
                  UserName = a.UserName,
                  ProfilePicture = a.ProfilePicture,
                  Name = a.Name,
                  PhoneNumber = a.PhoneNumber,
                  FullAddress = a.Address.FullAddress ,
                  LocalGovernment = a.Address.LocalGovernment,
                  State = a.Address.State,
                  Gender = a.Gender,
                  Email = a.Email,
                  Password = a.Password,
                  Role = a.Role,
                  IsActive = a.IsActive,
                  DateCreated = a.DateCreated,
                  DateModified = a.DateModified
            }).ToList();
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
              var user = users.Select(a => new UserDto{
                  Id = a.Id,
                  UserName = a.UserName,
                  ProfilePicture = a.ProfilePicture,
                  Name = a.Name,
                  PhoneNumber = a.PhoneNumber,
                  FullAddress = a.Address.FullAddress ,
                  LocalGovernment = a.Address.LocalGovernment,
                  State = a.Address.State,
                  Gender = a.Gender,
                  Email = a.Email,
                  Password = a.Password,
                  Role = a.Role,
                  IsActive = a.IsActive,
                  DateCreated = a.DateCreated,
                  DateModified = a.DateModified
            }).ToList();
            return new BaseResponse<IEnumerable<UserDto>>
            {
                Message = "List of Users 😎",
                IsSucess = true,
                Data = user
            };
        }

        public BaseResponse<UserDto> VerifyUser(string userEmail)
        {
           var user =  _userRepository.GetByEmailAsync(userEmail);
           user.IsRegistered = true;
           _userRepository.Update(user);
            return new BaseResponse<UserDto>
            {
                Message = "User Updated successfully",
                IsSucess = true
            };
        }
    }
}