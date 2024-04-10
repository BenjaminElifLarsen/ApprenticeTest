using CateringDataProcessingPlatform.DL.Models;
using Microsoft.EntityFrameworkCore;

namespace Catering.Shared.IPL.Context;

public class CateringContext : DbContext
{
    public CateringContext(DbContextOptions<CateringContext> options) : base(options)
    {
    }

    internal DbSet<Customer> Customers { get; set; }
    internal DbSet<Dish> Dishes { get; set; }
    internal DbSet<Menu> Menus { get; set; }
    internal DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(e =>
        {
            e.ComplexProperty(e => e.Location);
            e.ComplexProperty(e => e.Orders);

        });

        modelBuilder.Entity<Dish>(e =>
        {
            e.ComplexProperty(e => e.Menues);
        });

        modelBuilder.Entity<Menu>(e =>
        {
             e.ComplexProperty(e => e.Parts); //not sure if collection like this will work
        });


        modelBuilder.Entity<Order>(e =>
        {
            e.ComplexProperty(e => e.Customer);
            e.ComplexProperty(e => e.Menu);
            e.ComplexProperty(e => e.Time);
        });
    }
}
