using Microsoft.AspNetCore.Mvc; // [BindProperty], IActionResult
using Microsoft.AspNetCore.Mvc.RazorPages; // PageModel

using Packt.Shared;

namespace Northwind.Web.Pages;

public class SuppliersModel : PageModel
{
    public IEnumerable<Supplier>? Suppliers { get; set; }
    
    [BindProperty]
    public Supplier Supplier { get; set; } = null!;

    public SuppliersModel(NorthwindContext db)
    {
        this.db = db;
    }

    public void OnGet()
    {
        ViewData["Title"] = "Northwind B2B - Suppliers";
        Suppliers = db.Suppliers
            .OrderBy(c => c.Country)
            .ThenBy(c => c.CompanyName)
            .ToArray();
    }

    public IActionResult OnPost()
    {
        //return Page(); // return to original page

        if (Supplier is not null && ModelState.IsValid)
        {
            db.Suppliers.Add(Supplier);
            db.SaveChanges();
            return RedirectToPage("/suppliers");
        }
        else
        {
            return Page(); // return to original page
        }
    }

    private NorthwindContext db;
}
