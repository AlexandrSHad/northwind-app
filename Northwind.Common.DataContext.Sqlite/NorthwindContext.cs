using Microsoft.EntityFrameworkCore;

namespace Packt.Shared;

public partial class NorthwindContext : DbContext
{
    public NorthwindContext()
    {
    }

    public NorthwindContext(DbContextOptions<NorthwindContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; } = null!;

    public virtual DbSet<Customer> Customers { get; set; } = null!;

    public virtual DbSet<Employee> Employees { get; set; } = null!;

    public virtual DbSet<EmployeeTerritory> EmployeeTerritories { get; set; } = null!;

    public virtual DbSet<Order> Orders { get; set; } = null!;

    public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;

    public virtual DbSet<Product> Products { get; set; } = null!;

    public virtual DbSet<Shipper> Shippers { get; set; } = null!;

    public virtual DbSet<Supplier> Suppliers { get; set; } = null!;

    public virtual DbSet<Territory> Territories { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string dir = Environment.CurrentDirectory;
            string path = string.Empty;
            if (dir.EndsWith("net7.0"))
            {
                // Visual Studio
                // Running in the <project>\bin\<Debug|Release>\net7.0 directory.
                path = Path.Combine("..", "..", "..", "..", "Database", "Northwind.db");
            }
            else
            {
                // VSC
                // Running in the <project> directory.
                path = Path.Combine("..", "Database", "Northwind.db");
            }
            optionsBuilder.UseSqlite($"Filename={path}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Freight).HasDefaultValueSql("0");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.Quantity).HasDefaultValueSql("1");
            entity.Property(e => e.UnitPrice).HasDefaultValueSql("0");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Discontinued).HasDefaultValueSql("0");
            entity.Property(e => e.ReorderLevel).HasDefaultValueSql("0");
            entity.Property(e => e.UnitPrice).HasConversion<double>().HasDefaultValueSql("0");
            entity.Property(e => e.UnitsInStock).HasDefaultValueSql("0");
            entity.Property(e => e.UnitsOnOrder).HasDefaultValueSql("0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
