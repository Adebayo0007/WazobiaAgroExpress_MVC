using System.Security.Claims;
using Agro_Express.Dtos.Admin;
using Agro_Express.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agro_Express.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService) =>
            _adminService = adminService;
          //[Authorize(Roles = "Admin")]
        public IActionResult AdminIndex() => View();
        

        // [Authorize(Roles = "Admin")]
         public async Task<IActionResult> AdminProfile(string adminEmail)
        {
            adminEmail = User.FindFirst(ClaimTypes.Email).Value;
            var admin = await _adminService.GetByEmailAsync(adminEmail);
            if(admin.IsSucess == false)
            {
                TempData["error"] = admin.Message;
                return View();
            }
            TempData["success"] = admin.Message;
            return View(admin);
        }
        //  [Authorize(Roles = "Admin")]
         [HttpGet]
         public async Task<IActionResult> UpdateAdmin(string adminEmail)
        {
            if(adminEmail == null)adminEmail = User.FindFirst(ClaimTypes.Email).Value;
            var admin = await _adminService.GetByEmailAsync(adminEmail);
             if(admin.IsSucess == false)
            {
                TempData["error"] = admin.Message;
                return View();
            }
            TempData["success"] = admin.Message;
            return View(admin);
        }

        //  [Authorize]
          [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> UpdateAdmin(UpdateAdminRequestModel requestModel)
        {
            if(requestModel.Email == null)
            {
                requestModel.Email = User.FindFirst(ClaimTypes.Email).Value;
            }
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var admin = await _adminService.UpdateAsync(requestModel,id);
             if(admin.IsSucess == false)
            {
                TempData["error"] = admin.Message;
                return View();
            }
            TempData["success"] = admin.Message;
            return RedirectToAction(nameof(AdminProfile));
        }
         

        //  [Authorize(Roles = "Admin")]
         public async Task<IActionResult> DeleteAdmin(string adminEmail)
        {       
            if(adminEmail == null)adminEmail = User.FindFirst(ClaimTypes.Email).Value;
            var admin = await _adminService.GetByEmailAsync(adminEmail);
             if(admin.IsSucess == false)
            {
                TempData["error"] = admin.Message;
                return View();
            }
            TempData["success"] = admin.Message;
            return View(admin);      
        }
         // [Authorize]
        [HttpPost , ActionName("DeleteAdmin")]
        [ValidateAntiForgeryToken]
         public IActionResult DeleteAdminConfirmed(string adminId)
        {
            if(adminId == null)adminId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _adminService.DeleteAsync(adminId);
            TempData["success"] = "Admin Deleted successfully";
            return RedirectToAction("LogIn", "User");
        }
         
         // [Authorize(Roles = "Admin")]
        public IActionResult MyOffers() => View();
    }
}