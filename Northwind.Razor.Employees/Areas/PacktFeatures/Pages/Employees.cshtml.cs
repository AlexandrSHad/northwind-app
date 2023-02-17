using Microsoft.AspNetCore.Mvc.RazorPages; // PageModel
using Packt.Shared; // Employee, NorthwindContext

namespace PacktFeatures.Pages;

public class EmployeesPageModel : PageModel
{
    public IEnumerable<Employee> Employees { get; set; } = null!;

    public EmployeesPageModel(NorthwindContext db)
    {
        this.db = db;
    }

    public void OnGet()
    {
        ViewData["Title"] = "Northwind B2B - Employees";
        Employees = db.Employees
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToList();
    }

    private NorthwindContext db;
}