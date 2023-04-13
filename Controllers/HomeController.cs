using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Agro_Express.Models;
using Agro_Express.Services.Interfaces;

namespace Agro_Express.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productSercice;

    public HomeController(ILogger<HomeController> logger, IProductService productSercice)
    {
        _logger = logger;
        _productSercice = productSercice;
    }

    public async Task<IActionResult> Index()
    {
         await _productSercice.DeleteExpiredProducts();
        var products = await _productSercice.GetAllFarmProductAsync();
         if(products.IsSucess == false)
            {
                TempData["error"] = products.Message;
                return View();
            }
              TempData["success"] = products.Message;
            return View(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
