﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Agro_Express.Models;
using Agro_Express.Services.Interfaces;

namespace Agro_Express.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productSercice;
    private readonly IFarmerService _farmerSercice;

    public HomeController(ILogger<HomeController> logger, IProductService productSercice, IFarmerService farmerSercice)
    {
        _logger = logger;
        _productSercice = productSercice;
        _farmerSercice = farmerSercice;
    }

    public async Task<IActionResult> Index()
    {
         await _productSercice.DeleteExpiredProducts();
         await _farmerSercice.FarmerMonthlyDueUpdate();
        var products = await _productSercice.GetAllFarmProductAsync();
         if(products.IsSucess == false)
            {
                TempData["error"] = products.Message;
                return View();
            }
              TempData["success"] = products.Message;
            return View(products);
    }

    public IActionResult Policy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
