using System.Security.Claims;
using Agro_Express.Dtos.Farmer;
using Agro_Express.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agro_Express.Controllers
{
    public class FarmerController : Controller
    {
        private readonly IFarmerService _farmerService;
        private readonly IUserService _userService;
        public FarmerController(IFarmerService farmerService, IUserService userService)
        {
            _farmerService = farmerService;
            _userService = userService;
            
        }
         public IActionResult FarmerIndex()
        {
            return View();
        }

        //   [Authorize(Roles = "Manager,CEO")]
        public IActionResult CreateFarmer()
        {
            return View();
        }
        // [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> CreateFarmer(CreateFarmerRequestModel farmerModel)
        {
            
                 var farmerExist = await _userService.ExistByEmailAsync(farmerModel.Email);
                 if(!(farmerExist))
                 {

                    try{

                        IFormFile file = Request.Form.Files.FirstOrDefault();
                        using (var dataStream = new MemoryStream())
                        {
                           await file.CopyToAsync(dataStream);
                            farmerModel.ProfilePicture = dataStream.ToArray();
                        }
                    
                       }
                        catch(Exception ex)
                        {
                            TempData["error"] = $"Profile picture is required";
                        }
                        var farmer = await _farmerService.CreateAsync(farmerModel);

                    if(farmer.Message != null)
                    {
                        TempData["success"] = farmer.Message;
                    }
                        return RedirectToAction("LogIn", "User");
                 }
                 TempData["error"] = "Email already exist";
                  return View();
            
        }  

         public async Task<IActionResult> FarmerProfile(string farmerEmail)
        {
            farmerEmail = User.FindFirst(ClaimTypes.Email).Value;
            var farmer = await _farmerService.GetByEmailAsync(farmerEmail);
             if(farmer.IsSucess == false)
            {
                TempData["error"] = farmer.Message;
                return View();
            }
             TempData["success"] = farmer.Message;
            return View(farmer);
        }  

        [HttpGet]
         public async Task<IActionResult> UpdateFarmer(string farmerEmail)
        {
            if(farmerEmail == null)farmerEmail = User.FindFirst(ClaimTypes.Email).Value;
            var farmer = await _farmerService.GetByEmailAsync(farmerEmail);
             if(farmer.IsSucess == false)
            {
                TempData["error"] = farmer.Message;
                return View();
            }
             TempData["success"] = farmer.Message;
            return View(farmer);
        }
        [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> UpdateFarmer(UpdateFarmerRequestModel requestModel)
        {
             if(requestModel.Email == null) requestModel.Email = User.FindFirst(ClaimTypes.Email).Value;
            
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
             var farmer = await _farmerService.UpdateAsync(requestModel,id);
              if(farmer.IsSucess == false)
            {
                TempData["error"] = farmer.Message;
                return View();
            }
             TempData["success"] = farmer.Message;
            return RedirectToAction(nameof(FarmerProfile));
        }

          public async Task<IActionResult> DeleteFarmer(string farmerEmail)
        {       
            if(farmerEmail == null)farmerEmail = User.FindFirst(ClaimTypes.Email).Value;
            var farmer = await _farmerService.GetByEmailAsync(farmerEmail);
             if(farmer.IsSucess == false)
            {
                TempData["error"] = farmer.Message;
                return View();
            }
             TempData["success"] = farmer.Message;
            return View(farmer);      
        }
         
        [HttpPost , ActionName("DeleteFarmer")]
        [ValidateAntiForgeryToken]
         public IActionResult DeleteFarmerConfirmed(string farmerId)
        {
            if(farmerId == null)farmerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _farmerService.DeleteAsync(farmerId);
            TempData["success"] = "Farmer Deleted successfully";
            if(User.FindFirst(ClaimTypes.Role).Value == "Admin")
            {
                return RedirectToAction(nameof(Farmers));
            }
            return RedirectToAction("LogIn", "User");
        }

         public async Task<IActionResult> Farmers()
        {
             var farmers = await _farmerService.GetAllActiveAndNonActiveAsync();
            if(farmers.IsSucess == false)
            {
                TempData["error"] = farmers.Message;
                return View();
            }
            return View(farmers);

        }

          [HttpPost]
         public async Task<IActionResult> SearchFarmer(string searchInput)
        {
              if(searchInput == null || searchInput == "")
            {
                return BadRequest();
            }
             var farmers = await _farmerService.SearchFarmerByEmailOrUserName(searchInput);
              if(farmers.IsSucess == false)
            {
                TempData["error"] = farmers.Message;
                return View();
            }
             TempData["success"] = farmers.Message;
            return View(farmers);
        }

         public IActionResult MyOffers()
        {
            return View();
        }
    }
}