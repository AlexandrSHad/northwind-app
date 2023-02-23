using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Mvc.Models;
using Packt.Shared;

namespace Northwind.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly NorthwindContext _db;
    private readonly ILogger<HomeController> _logger;

    public HomeController(NorthwindContext db, ILogger<HomeController> logger)
    {
        _db = db;
        _logger = logger;
    }

    [ResponseCache(Duration = 10 /* seconds */, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Index()
    {
        HomeIndexViewModel model = new
        (
            VisitorCount: Random.Shared.Next(1, 1001),
            Categories: await _db.Categories.ToListAsync(),
            Products: await _db.Products.ToListAsync()
        );

        return View(model); // pass model to view
    }

    [Authorize(Roles = "Administrators")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    public async Task<IActionResult> ProductDetail(int? id, string alertstyle = "success")
    {
        ViewData["alertstyle"] = alertstyle;

        if (!id.HasValue)
        {
            return BadRequest("You must pass a product ID in the route, for example, / Home / ProductDetail / 21");
        }

        Product? model = await _db.Products.SingleOrDefaultAsync(p => p.ProductId == id);

        if (model is null)
        {
            return NotFound($"ProductId {id} not found.");
        }

        return View(model); // pass model to view and then return result
    }

    public async Task<IActionResult> ProductsThatCostMoreThan(decimal? price)
    {
        if (!price.HasValue)
        {
            return BadRequest("You must pass a product price in the query string, for example, /Home/ProductsThatCostMoreThan?price=50");
        }

        IEnumerable<Product> model = await _db.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.UnitPrice > price)
            .ToListAsync();
        
        if (!model.Any())
        {
            return NotFound($"No products cost more than {price:C}.");
        }
        
        ViewData["MaxPrice"] = price.Value.ToString("C");
        return View(model); // pass model to view
    }
}
