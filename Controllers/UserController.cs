using System.Security.Claims;
using Agro_Express.Dtos.User;
using Agro_Express.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Agro_Express.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
            
        }

         public IActionResult LogIn()
        {
            return View();

        }

        [HttpPost, ActionName("LogIn")]
        public async Task<IActionResult> LogInConfirmed(LogInRequestModel loginModel)
        {
            if (loginModel.Email == null || loginModel.Password == null)
            {
                return NotFound();
            }
            var user = await _userService.Login(loginModel);
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
            if (user.Data.Role == "Farmer")
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

        public IActionResult SignUpPage()
        {
            return View();
        }

        public async Task<IActionResult> ApplicationUsers()
        {
            var users = await _userService.GetAllAsync();
            if(users.IsSucess == false)
            {
                TempData["error"] = users.Message;
                return View();
            }
            return View(users);

        }

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

           [HttpPost]
         public async Task<IActionResult> SearchUser(string searchInput)
        {
              if(searchInput == null || searchInput == "")
            {
                return BadRequest();
            }
             var users = await _userService.SearchUserByEmailOrUserName(searchInput);
              if(users.IsSucess == false)
            {
                TempData["error"] = users.Message;
                return View();
            }
             TempData["success"] = users.Message;
            return View(users);
        }

        public async Task<IActionResult> PendingRegistration()
        {
            var pendingRequests = await _userService.PendingRegistration();
              if(pendingRequests.IsSucess == false)
            {
                TempData["error"] = pendingRequests.Message;
                return View();
            }
             TempData["success"] = pendingRequests.Message;
            return View(pendingRequests);
        }
        

        public IActionResult VerifyUser(string userEmail)
        {
            if(userEmail != null)_userService.VerifyUser(userEmail);
            return RedirectToAction(nameof(PendingRegistration));
        }
    }
}