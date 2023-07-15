using System.Security.Claims;
using Agro_Express.Dtos.Buyer;
using Agro_Express.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agro_Express.Controllers
{
    public class BuyerController : Controller
    {
           private readonly IBuyerService _buyerService;
           private readonly IUserService _userService;
        public BuyerController(IBuyerService buyerService, IUserService userService)
        {
            _buyerService = buyerService;
            _userService = userService;  
        }
         public IActionResult BuyerPolicy() => View();

        [Authorize(Roles = "Buyer")]
        public IActionResult BuyerIndex() => View();
     
        public IActionResult CreateBuyer() => View();
      
        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> CreateBuyer(CreateBuyerRequestModel buyerModel)
        {
            var buyerExist = await _userService.ExistByEmailAsync(buyerModel.Email);
            if(!(buyerExist))
            {

                    try{

                        IFormFile file = Request.Form.Files.FirstOrDefault();
                        using (var dataStream = new MemoryStream())
                        {
                           await file.CopyToAsync(dataStream);
                            buyerModel.ProfilePicture = dataStream.ToArray();
                        }
                    
                       }
                        catch(Exception ex)
                        {
                            TempData["error"] = $"Profile picture is required";
                        }
                        var buyer = await _buyerService.CreateAsync(buyerModel);

                        if(buyer.IsSucess == false)
                        {
                             TempData["error"] = buyer.Message;
                             return View();
                        }

                    if(buyer.Message != null)
                    {
                        TempData["success"] = buyer.Message;
                    }
                 
                        return RedirectToAction("LogIn", "User");
            }
            TempData["error"] = "Email already exist";
            return View();   
        }  

          
          [Authorize(Roles = "Buyer")]
          public async Task<IActionResult> BuyerProfile(string buyerEmail)
        {
            buyerEmail = User.FindFirst(ClaimTypes.Email).Value;
            var buyer = await _buyerService.GetByEmailAsync(buyerEmail);
             if(buyer.IsSucess == false)
            {
                TempData["error"] = buyer.Message;
                return View();
            }
             TempData["success"] = buyer.Message;
            return View(buyer);
        } 

        
         [Authorize(Roles = "Buyer")]
        [HttpGet]
         public async Task<IActionResult> UpdateBuyer(string buyerEmail)
        {
            if(buyerEmail == null)buyerEmail = User.FindFirst(ClaimTypes.Email).Value;
            var buyer = await _buyerService.GetByEmailAsync(buyerEmail);
             if(buyer.IsSucess == false)
            {
                TempData["error"] = buyer.Message;
                return View();
            }
             TempData["success"] = buyer.Message;
            return View(buyer);
        }


          [Authorize]
          [HttpPost]
          [ValidateAntiForgeryToken]
         public async Task<IActionResult> UpdateBuyer(UpdateBuyerRequestModel requestModel)
        {
             if(requestModel.Email == null)
            {
                requestModel.Email = User.FindFirst(ClaimTypes.Email).Value;
            }
            if(requestModel.ProfilePicture != null)
            {
                 IFormFile file = Request.Form.Files.FirstOrDefault();
                        using (var dataStream = new MemoryStream())
                        {
                           await file.CopyToAsync(dataStream);
                            requestModel.ProfilePicture = dataStream.ToArray();
                        }
            }
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
             var buyer = await _buyerService.UpdateAsync(requestModel,id);
              if(buyer.IsSucess == false)
            {
                TempData["error"] = buyer.Message;
                return View();
            }
             TempData["success"] = buyer.Message;
            return RedirectToAction(nameof(BuyerProfile));
        }

           
            [Authorize(Roles = "Buyer, Admin")]
            public async Task<IActionResult> DeleteBuyer(string buyerEmail)
        {       
            if(buyerEmail == null)buyerEmail = User.FindFirst(ClaimTypes.Email).Value;
            var buyer = await _buyerService.GetByEmailAsync(buyerEmail);
             if(buyer.IsSucess == false)
            {
                TempData["error"] = buyer.Message;
                return View();
            }
             TempData["success"] = buyer.Message;
            return View(buyer);      
        }

         
        [Authorize]
        [HttpPost , ActionName("DeleteBuyer")]
        [ValidateAntiForgeryToken]
         public IActionResult DeleteBuyerConfirmed(string buyerId)
        {
            if(buyerId == null)buyerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _buyerService.DeleteAsync(buyerId);
             TempData["success"] = "Buyer Deleted successfully";
             if(User.FindFirst(ClaimTypes.Role).Value == "Admin")
            {
                return RedirectToAction(nameof(Buyers));
            }
            return RedirectToAction("LogIn", "User");
        }
        
         
          [Authorize(Roles = "Admin")]
          public async Task<IActionResult> Buyers()
        {
             var buyers = await _buyerService.GetAllActiveAndNonActiveAsync();
            if(buyers.IsSucess == false)
            {
                TempData["error"] = buyers.Message;
                return View();
            }
            return View(buyers);

        }


          [Authorize(Roles = "Admin")]
          [HttpPost]
         public async Task<IActionResult> SearchBuyer(string searchInput)
        {
               if(string.IsNullOrWhiteSpace(searchInput))
            {
                 return BadRequest();
            }
             
             var buyers = await _buyerService.SearchBuyerByEmailOrUserName(searchInput);
              if(buyers.IsSucess == false)
            {
                TempData["error"] = buyers.Message;
                return View();
            }
             TempData["success"] = buyers.Message;
            return View(buyers);
        }
        
    }
}