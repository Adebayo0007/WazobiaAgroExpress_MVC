using System.Security.Claims;
using Agro_Express.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agro_Express.Controllers
{
    public class RequestedProductController : Controller
    {
         private readonly IRequestedProductService _requstedProductSercice;
        public RequestedProductController(IRequestedProductService requstedProductSercice)
        {
            _requstedProductSercice = requstedProductSercice;
        }
        public async Task<IActionResult> CreateRequestedProduct(string productId)
        {
          var product =  await  _requstedProductSercice.CreateRequstedProductAsync(productId);
          if(product.IsSucess != true)
          {
             TempData["error"] = "internal error";
             return BadRequest();
          }
           TempData["order"] = product.Message;
            return RedirectToAction("AvailableProducts", "Product");
         }

          public async Task<IActionResult> OrderedProductAndPendingProduct(string buyerEmail)
        {
                    if(buyerEmail == null)buyerEmail = User.FindFirst(ClaimTypes.Email).Value;
                var results =  await  _requstedProductSercice.OrderedAndPendingProduct(buyerEmail);
                if(results.IsSucess != true)
                {
                    TempData["error"] = "internal error";
                    return BadRequest();
                }
                TempData["success"] = results.Message;
                    return View(results);
         }


         public async Task<IActionResult> DeleteRequest(string requestId)
         {
            if(requestId != null)
            {
                TempData["success"] = "Order deleted successfully 😎";
                await _requstedProductSercice.DeleteRequestedProduct(requestId);
                return RedirectToAction(nameof(OrderedProductAndPendingProduct));
            }
            TempData["error"] = "internal error 🙄";
              return RedirectToAction(nameof(OrderedProductAndPendingProduct));
            

         }

         public async Task<IActionResult> DeliveredRequest(string requestId)
         {
            if(requestId != null)
            {
                await _requstedProductSercice.ProductDelivered(requestId);
                TempData["success"] = "Order delivered successfully 😎";
                return RedirectToAction(nameof(OrderedProductAndPendingProduct));
            }
            TempData["error"] = "internal error 🙄";
              return RedirectToAction(nameof(OrderedProductAndPendingProduct));
            

         }
              [Authorize(Roles = "Farmer")]
           public async Task<IActionResult> AcceptRequest(string requestId)
         {
            if(requestId != null)
            {
                var product = await _requstedProductSercice.ProductAccepted(requestId);
                TempData["success"] = product.Message+" 😎";
                return RedirectToAction(nameof(MyRequests));
            }
            TempData["error"] = "internal error 🙄";
              return RedirectToAction(nameof(MyRequests));
            

         }


         [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> MyRequests(string farmerId)
        {
            farmerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
           var requests =  await _requstedProductSercice.MyRequests(farmerId); 
            if(requests.IsSucess == false)
            {
                TempData["error"] = requests.Message;
                return View();
            }
            TempData["success"] = requests.Message;
           return View(requests);
        }

    }
}