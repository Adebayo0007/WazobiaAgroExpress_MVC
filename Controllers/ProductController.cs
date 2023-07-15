using Agro_Express.Dtos.Product;
using Agro_Express.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agro_Express.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productSercice;
        public ProductController(IProductService productSercice) =>  _productSercice = productSercice;
        [Authorize(Roles = "Farmer")]
        public IActionResult CreateProduct() => View();
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductRequestModel model)
        {
                  try{

                        IFormFile file1 = Request.Form.Files.FirstOrDefault();
                        using (var dataStream1 = new MemoryStream())
                        {
                           file1.OpenReadStream();
                           await file1.CopyToAsync(dataStream1);
                            model.FirstDimentionPicture = dataStream1.ToArray();
                            model.SecondDimentionPicture = dataStream1.ToArray();
                            model.ThirdDimentionPicture = dataStream1.ToArray();
                            model.ForthDimentionPicture = dataStream1.ToArray();
                        }
                    
                       }
                        catch(Exception ex)
                        {
                            TempData["error"] = "Internal error, Check your pictures"; 
                            return View();
                        }

                       
            var product = await _productSercice.CreateProductAsync(model);
            if(product.IsSucess == false)
            {
                TempData["error"] = product.Message;
                return View();
            }
            return RedirectToAction(nameof(MyProducts));
        }
         
          [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> MyProducts()
        {
           var products =  await _productSercice.GetFarmerFarmProductsByIdAsync();
            if(products.IsSucess == false)
            {
                TempData["error"] = products.Message;
                return View();
            }
            TempData["success"] = products.Message;
           return View(products);
        }
           public async Task<IActionResult> AvailableProducts()
        {
           var products =  await _productSercice.GetAllFarmProductByLocationAsync();
            if(products.IsSucess == false)
            {
                TempData["error"] = products.Message;
                return View();
            }
            TempData["success"] = products.Message;
           return View(products);
        }
          
          [Authorize(Roles = "Farmer")]
          [HttpGet]
         public async Task<IActionResult> UpdateProduct(string productId)
        {
            var product = await _productSercice.GetProductById(productId);
             if(product.IsSucess == false)
            {
                TempData["error"] = product.Message;
                return View();
            }
             TempData["success"] = product.Message;
            return View(product);
        }

        [Authorize]
        [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> UpdateProduct(UpdateProductRequestModel requestModel,string productsId)
        {
             var product = await _productSercice.UpdateProduct(requestModel,productsId);
              if(product.IsSucess == false)
            {
                TempData["error"] = product.Message;
                return View();
            }
             TempData["success"] = product.Message;
            return RedirectToAction(nameof(MyProducts));
        }
            
            [Authorize(Roles = "Farmer")]
          public async Task<IActionResult> DeleteProduct(string productId)
        {       
           var product = await _productSercice.GetProductById(productId);
             if(product.IsSucess == false)
            {
                TempData["error"] = product.Message;
                return View();
            }
             TempData["success"] = product.Message;
            return View(product);
        }
         [Authorize]
        [HttpPost , ActionName("DeleteProduct")]
         public async Task<IActionResult> DeleteProductConfirmed(string productsId)
        {
            if(productsId == null)
            {
                TempData["error"] = "Product not deleted,internal error 🙄";
            }
            await _productSercice.DeleteProduct(productsId);
            TempData["success"] = "Product Deleted successfully 😎";
            return RedirectToAction(nameof(MyProducts));
        }


         [HttpPost]
         public async Task<IActionResult> SearchProduct(string searchInput)
        {
            if(string.IsNullOrWhiteSpace(searchInput))
            {
                 return BadRequest();
            }
             var products = await _productSercice.SearchProductsByProductNameOrFarmerUserNameOrFarmerEmail(searchInput);
              if(products.IsSucess == false)
            {
                TempData["error"] = products.Message;
                return View();
            }
             TempData["success"] = products.Message;
            return View(products);
        }

        public IActionResult ThumbUp(string productId)
        {
            if(productId != null)
            {
              _productSercice.ThumbUp(productId);
              TempData["success"] = "Liked 👍";
            }
              return RedirectToAction(nameof(AvailableProducts));
        }

          public IActionResult ThumbDown(string productId)
        {
            if(productId != null)
            {
              _productSercice.ThumbDown(productId);
              TempData["success"] = "Thumb down 👎";
            }
              return RedirectToAction(nameof(AvailableProducts));
        }  
    }
}