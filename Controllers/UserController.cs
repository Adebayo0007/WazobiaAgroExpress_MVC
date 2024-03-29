using System.Security.Claims;
using Agro_Express.Dtos.User;
using Agro_Express.Dtos;
using Agro_Express.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Agro_Express.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMemoryCache _memoryCache;
        public UserController(IUserService userService, IMemoryCache memoryCache) 
        {
            _userService = userService;
            _memoryCache = memoryCache;
         }

         public IActionResult LogIn() => View();

        [HttpPost, ActionName("LogIn")]
        public async Task<IActionResult> LogInConfirmed(LogInRequestModel loginModel)
        {
            if (loginModel.Email == null || loginModel.Password == null)
            {
                return NotFound();
            }
            var user = await _userService.Login(loginModel);
             if (user.IsSucess == false && user.Message == "Due")
            {
                        TempData["error"] = "kindly pay up your due";
                        return RedirectToAction("PayMonthlyDue", "Farmer");
             }

            if (user.IsSucess == false)
            {
                TempData["error"] = user.Message;
                return View();
            }
              

             var roles = new List<string>();
             var claims = new List<Claim>();
            if(user.Data.Role == "Admin")
             {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Data.Id));
                claims.Add(new Claim(ClaimTypes.Email, user.Data.Email));
                 claims.Add(new Claim(ClaimTypes.Anonymous, user.Data.Name));
             }
                if(user.Data.Role == "Farmer")
             {
                claims.Add(new Claim(ClaimTypes.Role, "Farmer"));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Data.Id));
                claims.Add(new Claim(ClaimTypes.Email, user.Data.Email));
                 claims.Add(new Claim(ClaimTypes.Anonymous, user.Data.Name));
             }
               if(user.Data.Role == "Buyer")
             {
                claims.Add(new Claim(ClaimTypes.Role, "Buyer"));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Data.Id));
                claims.Add(new Claim(ClaimTypes.Email, user.Data.Email));
                 claims.Add(new Claim(ClaimTypes.Anonymous, user.Data.Name));
             }
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authenticationProperties = new AuthenticationProperties();
            var principal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);


            if (user.Data.Role == "Admin")
            {
                TempData["success"] = user.Message;
                return RedirectToAction("AdminIndex", "Admin");
            }

            if (user.Data.Role == "Farmer" && user.Data.Due == true)
            {
                    TempData["success"] = user.Message;
                    return RedirectToAction("FarmerIndex", "Farmer");
            }
            if (user.Data.Role == "Buyer")
            {
                TempData["success"] = user.Message;
                return RedirectToAction("BuyerIndex", "Buyer");

            }
           
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> LogOut(string email)
        {
             email = User.FindFirst(ClaimTypes.Email).Value;
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["success"] = $"{email} Logged out Successfully";
            TempData.Keep();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult SignUpPage() => View();

         [Authorize(Roles = "Admin")]
         [ResponseCache(Duration = 3600,Location = ResponseCacheLocation.Any)]  //using cache as an attribute for fast performance
        public async Task<IActionResult> ApplicationUsers()
        {
              if (!_memoryCache.TryGetValue($"Application_Users", out BaseResponse<IEnumerable<UserDto>> users))
            {
                 users =  await _userService.GetAllAsync();
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                };
                 _memoryCache.Set($"Application_Users", users, cacheEntryOptions);
            }
          
            if(users.IsSucess == false)
            {
                TempData["error"] = users.Message;
                return View();
            }
            return View(users);

        }
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {       
           var user = await _userService.GetByIdAsync(userId);
             if(user.IsSucess == false)
            {
                TempData["error"] = user.Message;
                return View();
            }
             TempData["success"] = user.Message;
            return View(user);
        }
         [Authorize]
        [HttpPost , ActionName("DeleteUser")]
         public async Task<IActionResult> DeleteUserConfirmed(string usersId)
        {
            if(usersId == null)
            {
                TempData["error"] = "User not deleted,internal error 🙄";
            }
            await _userService.DeleteAsync(usersId);
            TempData["success"] = "User Deleted successfully 😎";
            return RedirectToAction(nameof(ApplicationUsers));
        }
           [Authorize(Roles = "Admin")]
           [HttpPost]
         public async Task<IActionResult> SearchUser(string searchInput)
        {
             if(string.IsNullOrWhiteSpace(searchInput))
            {
                 return BadRequest();
            }

              if (!_memoryCache.TryGetValue($"Searched_User_{searchInput}", out BaseResponse<IEnumerable<UserDto>> users))
            {
                 users =  await _userService.SearchUserByEmailOrUserName(searchInput);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                };
                 _memoryCache.Set($"Searched_User_{searchInput}", users, cacheEntryOptions);
            }

              if(users.IsSucess == false)
            {
                TempData["error"] = users.Message;
                return View();
            }
             TempData["success"] = users.Message;
            return View(users);
        }
          
          [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PendingRegistration()
        {
             if (!_memoryCache.TryGetValue($"Pending_Registration", out BaseResponse<IEnumerable<UserDto>> pendingRequests))
            {
                 pendingRequests =  await _userService.PendingRegistration();
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                };
                 _memoryCache.Set($"Pending_Registration", pendingRequests, cacheEntryOptions);
            }
           
              if(pendingRequests.IsSucess == false)
            {
                TempData["error"] = pendingRequests.Message;
                return View();
            }
             TempData["success"] = pendingRequests.Message;
            return View(pendingRequests);
        }
        
        [Authorize(Roles = "Admin")]
        public IActionResult VerifyUser(string userEmail)
        {
            if(userEmail != null)_userService.VerifyUser(userEmail);
            return RedirectToAction(nameof(PendingRegistration));
        }

        [HttpGet]
         public IActionResult ForgottenPassword() =>  View();
          
        [HttpPost]
         public async Task<IActionResult> ForgottenPassword(string userEmail)
        {
            var response = await _userService.ForgottenPassword(userEmail);
            if(response == false)
            {
                return View();
            }
             return RedirectToAction(nameof(LogIn));
        }
    }
}