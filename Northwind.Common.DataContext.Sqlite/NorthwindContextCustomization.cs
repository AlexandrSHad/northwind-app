using Microsoft.EntityFrameworkCore;

namespace Packt.Shared;

public partial class NorthwindContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Category>()
        //     .Property(e => e.CategoryName).IsRequired();

        // modelBuilder.Entity<Customer>()
        //     .Property(e => e.CustomerId).HasAnnotation
    }
}