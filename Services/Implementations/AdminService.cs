using Agro_Express.Dtos;
using Agro_Express.Dtos.Admin;
using Agro_Express.Dtos.User;
using Agro_Express.Models;
using Agro_Express.Repositories.Interfaces;
using Agro_Express.Services.Interfaces;

namespace Agro_Express.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IUserService _userService;
         private readonly IUserRepository _userRepository;
        public AdminService(IAdminRepository adminRepository, IUserService userService, IUserRepository userRepository)
        {
            _adminRepository = adminRepository;
            _userService = userService;
            _userRepository = userRepository;
        }
        public async Task DeleteAsync(string adminId)
        {
            var admin = _userRepository.GetByIdAsync(adminId);
           if(admin.IsActive == true)
           {
             admin.IsActive = false;
           }
           else{

           admin.IsActive = true;
           }
            await _userRepository.Delete(admin);
        }

        public async Task<BaseResponse<IEnumerable<AdminDto>>> GetAllAsync()
        {
             var admins = await _adminRepository.GetAllAsync();

           if(admins == null)
            {
                return new BaseResponse<IEnumerable<AdminDto>>
                {
                    Message = "No admin Found 🙄",
                    IsSucess = false
                };  
            }
              var admin = admins.Select(a => AdminDto(a)).ToList();
            return new BaseResponse<IEnumerable<AdminDto>>
            {
                Message = "List of Admins 📔",
                IsSucess = true,
                Data = admin
            };
        }

        public async Task<BaseResponse<AdminDto>> GetByEmailAsync(string adminEmail)
        {
              var admin =  _adminRepository.GetByEmailAsync(adminEmail);
              if(admin == null)
              {
                        return new BaseResponse<AdminDto>
                    {
                        Message = "Admin not Found 🙄",
                        IsSucess = false
                    };
              }
              AdminDto adminDto = null;
            if(admin is not null)
            {
                adminDto = AdminDto(admin);
            }
            return new BaseResponse<AdminDto>
            {
                Message = "Admin Found successfully 😎",
                IsSucess = true,
                Data = adminDto
            };
        }

        public async Task<BaseResponse<AdminDto>> GetByIdAsync(string adminId)
        {
             var admin =  _adminRepository.GetByIdAsync(adminId);
               if(admin == null)
              {
                        return new BaseResponse<AdminDto>
                    {
                        Message = "Admin not Found 🙄",
                        IsSucess = false
                    };
              }
            var adminDto = AdminDto(admin);
            return new BaseResponse<AdminDto>
            {
                Message = "Admin Found successfully 😎",
                IsSucess = true,
                Data = adminDto
            };
        }

        public async Task<BaseResponse<AdminDto>> UpdateAsync(UpdateAdminRequestModel updateAdminModel, string id)
        {
              var updateAdmin = new UpdateUserRequestModel{
                UserName = updateAdminModel.UserName,
                Name = updateAdminModel.Name,
                PhoneNumber  = updateAdminModel.PhoneNumber,
                FullAddress = updateAdminModel.FullAddress,
                LocalGovernment = updateAdminModel.LocalGovernment,
                State  = updateAdminModel.State,
                Gender = updateAdminModel.Gender,
               Email = updateAdminModel.Email,
               Password = (updateAdminModel.Password) != null?BCrypt.Net.BCrypt.HashPassword(updateAdminModel.Password): null,
            };
            var user =  _userService.UpdateAsync(updateAdmin, id);

            if(user.IsSucess == false)
            {
                    return new BaseResponse<AdminDto>{
                    Message = "Admin not Updated, internal error 🙄",
                    IsSucess = false
                };

            }
              var admin = _adminRepository.GetByEmailAsync(updateAdmin.Email);
               if(admin == null)
            {
                    return new BaseResponse<AdminDto>{
                    Message = "Admin not Updated, internal error 🙄",
                    IsSucess = false
                };

            }
              _adminRepository.Update(admin);

              var adminDto = new AdminDto{
                UserName = updateAdminModel.UserName,
                Name = updateAdminModel.Name,
                PhoneNumber  = updateAdminModel.PhoneNumber,
                FullAddress = updateAdminModel.FullAddress,
                LocalGovernment = updateAdminModel.LocalGovernment,
                State  = updateAdminModel.State,
                Gender = updateAdminModel.Gender,
               Email = updateAdminModel.Email,
               Password = updateAdmin.Password
            };

            return new BaseResponse<AdminDto>{
                Message = "Admin Updated successfully 😎",
                IsSucess = true,
                Data = adminDto
            };
        }

        private AdminDto AdminDto(Admin admin) => 
            new AdminDto()
            {
                UserName = admin.User.UserName,
                Name = admin.User.Name,
                PhoneNumber = admin.User.PhoneNumber,
                FullAddress = admin.User.Address.FullAddress,
                LocalGovernment = admin.User.Address.LocalGovernment,
                State = admin.User.Address.State,
                Gender = admin.User.Gender,
                Email = admin.User.Email,
                Password = admin.User.Password,
                Role = admin.User.Role,
                IsActive = admin.User.IsActive,
                DateCreated = admin.User.DateCreated,
                DateModified = admin.User.DateModified

            };
    }
}