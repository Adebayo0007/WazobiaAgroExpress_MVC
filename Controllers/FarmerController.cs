using System.Security.Claims;
using Agro_Express.Dtos;
using Agro_Express.Dtos.AllFarmers;
using Agro_Express.Dtos.Farmer;
using Agro_Express.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Agro_Express.Controllers
{
    public class FarmerController : Controller
    {
        private readonly IFarmerService _farmerService;
        private readonly IUserService _userService;
        private readonly IMemoryCache _memoryCache;
        public FarmerController(IFarmerService farmerService, IUserService userService, IMemoryCache memoryCache)
        {
            _farmerService = farmerService;
            _userService = userService;
            _memoryCache = memoryCache;
            
        }

          public IActionResult FarmerPolicy() => View();

        [Authorize(Roles = "Farmer")]
         public IActionResult FarmerIndex() => View();

        public IActionResult CreateFarmer() => View();
       
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
                         if(farmer.IsSucess == false)
                        {
                             TempData["error"] = farmer.Message;
                             return View();
                        }

                    if(farmer.Message != null)
                    {
                        TempData["success"] = farmer.Message;
                    }
                        return RedirectToAction("LogIn", "User");
                 }
                 TempData["error"] = "Email already exist";
                  return View();
            
        }  
         
         [Authorize(Roles = "Farmer")]
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
         
         [Authorize(Roles = "Farmer")]
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
        
        [Authorize]
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
          
          [Authorize(Roles = "Farmer, Admin")]
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
         [Authorize]
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
         
         [Authorize(Roles = "Admin")]
         public async Task<IActionResult> Farmers()
        {
             if (!_memoryCache.TryGetValue($"Farmers", out BaseResponse<ActiveAndNonActiveFarmer> farmers))
            {
                 farmers =  await _farmerService.GetAllActiveAndNonActiveAsync();
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                };
                 _memoryCache.Set($"Farmers", farmers, cacheEntryOptions);

            }
            
            if(farmers.IsSucess == false)
            {
                TempData["error"] = farmers.Message;
                return View();
            }
            return View(farmers);

        }
        
         // [Authorize(Roles = "Admin")]
          [HttpPost]
         public async Task<IActionResult> SearchFarmer(string searchInput)
        {
               if(string.IsNullOrWhiteSpace(searchInput))
            {
                 return BadRequest();
            }

               if (!_memoryCache.TryGetValue($"Searched_Farmers_{searchInput}", out BaseResponse<IEnumerable<FarmerDto>> farmers))
            {
                 farmers =  await _farmerService.SearchFarmerByEmailOrUserName(searchInput);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                };
                 _memoryCache.Set($"Searched_Farmers_{searchInput}", farmers, cacheEntryOptions);

            }
        
              if(farmers.IsSucess == false)
            {
                TempData["error"] = farmers.Message;
                return View();
            }
             TempData["success"] = farmers.Message;
            return View(farmers);
        }
         
         [HttpGet]
        public IActionResult PayMonthlyDue() =>  View();

         [HttpGet]
        public IActionResult UpdateToHasPaid(string userEmail)
        {
              _farmerService.UpdateToHasPaidDue(userEmail);
                 return RedirectToAction("LogIn", "User");
        }

         [Authorize(Roles = "Farmer")]
         public IActionResult MyOffers() => View();

    }
}