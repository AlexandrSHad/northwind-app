using Microsoft.AspNetCore.Mvc.RazorPages;

using Packt.Shared;

namespace Northwind.Web.Pages;

public class CustomersModel : PageModel
{
    public List<Customer> Customers { get; set; } = null!;

    public CustomersModel(NorthwindContext db)
    {
        this.db = db;
    }

    public void OnGet()
    {
        Customers = db.Customers.ToList();
    }

    private NorthwindContext db;
}