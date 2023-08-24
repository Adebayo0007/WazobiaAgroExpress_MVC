using Agro_Express.Dtos.Product;
using Agro_Express.Dtos;
using System.Security.Claims;
using Agro_Express.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;


namespace Agro_Express.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productSercice;
        private readonly IMemoryCache _memoryCache;
        public ProductController(IProductService productSercice, IMemoryCache memoryCache) 
        {
         _productSercice = productSercice;
         _memoryCache = memoryCache;
        }
            
        [Authorize(Roles = "Farmer")]
        public IActionResult CreateProduct() => View();
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
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
           [ResponseCache(Duration = 3600,Location = ResponseCacheLocation.Any)] 
        public async Task<IActionResult> MyProducts()
        {
             var email = User.FindFirst(ClaimTypes.Email).Value;
             if (!_memoryCache.TryGetValue($"My_Products_{email}", out BaseResponse<IEnumerable<ProductDto>> myProduct))
            {
                 myProduct =  await _productSercice.GetFarmerFarmProductsByEmailAsync(email);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Cache for 5 minutes
                };
                 _memoryCache.Set($"My_Products_{email}", myProduct, cacheEntryOptions);

            }
           
            if(myProduct.IsSucess == false)
            {
                TempData["error"] = myProduct.Message;
                return View();
            }
            TempData["success"] = myProduct.Message;
           return View(myProduct);
        }
        
        [ResponseCache(Duration = 3600,Location = ResponseCacheLocation.Any)]  //using cache as an attribute 
           public async Task<IActionResult> AvailableProducts()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email).Value;
             if (!_memoryCache.TryGetValue($"Available_Products_{userEmail}", out BaseResponse<IEnumerable<ProductDto>> availableProduct))
            {
                 availableProduct =  await _productSercice.GetAllFarmProductByLocationAsync(userEmail);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                };
                 _memoryCache.Set($"Available_Products_{userEmail}", availableProduct, cacheEntryOptions);

            }
              
                    if(availableProduct.IsSucess == false)
                    {
                        TempData["error"] = availableProduct.Message;
                        return View();
                    }
                    TempData["success"] = availableProduct.Message;
           
                return View(availableProduct);
        }
          
          [Authorize(Roles = "Farmer")]
           [ResponseCache(Duration = 2000,Location = ResponseCacheLocation.Any)] 
          [HttpGet]
         public async Task<IActionResult> UpdateProduct(string productId)
        {
            if (!_memoryCache.TryGetValue($"Product_With_Id_{productId}", out BaseResponse<ProductDto> product))
            {
                 product =  await _productSercice.GetProductById(productId);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Cache for 5 minutes
                };
                 _memoryCache.Set($"Product_With_Id_{productId}", product, cacheEntryOptions);

            }
           
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
             [ResponseCache(Duration = 2000,Location = ResponseCacheLocation.Any)] 
          public async Task<IActionResult> DeleteProduct(string productId)
        {       
          if (!_memoryCache.TryGetValue($"Product_With_Id_{productId}", out BaseResponse<ProductDto> product))
            {
                 product =  await _productSercice.GetProductById(productId);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Cache for 5 minutes
                };
                 _memoryCache.Set($"Product_With_Id_{productId}", product, cacheEntryOptions);

            }
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
          [ResponseCache(Duration = 3600,Location = ResponseCacheLocation.Any)] 
         public async Task<IActionResult> SearchProduct(string searchInput)
        {
            if(string.IsNullOrWhiteSpace(searchInput))
            {
                 return BadRequest();
            }

             if (!_memoryCache.TryGetValue($"Sarched_Products_{searchInput}", out BaseResponse<IEnumerable<ProductDto>> searchedProduct))
            {
                 searchedProduct =  await _productSercice.SearchProductsByProductNameOrFarmerUserNameOrFarmerEmail(searchInput);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                };
                 _memoryCache.Set($"Sarched_Products_{searchInput}", searchedProduct, cacheEntryOptions);

            }
             
              if(searchedProduct.IsSucess == false)
            {
                TempData["error"] = searchedProduct.Message;
                return View();
            }
             TempData["success"] = searchedProduct.Message;
            return View(searchedProduct);
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