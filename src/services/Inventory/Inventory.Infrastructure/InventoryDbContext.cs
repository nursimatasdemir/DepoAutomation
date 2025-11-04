using Microsoft.EntityFrameworkCore;
using Inventory.Domain;
using Inventory.Application.Abstraction;
using Inventory.Domain.Views;

namespace Inventory.Infrastructure;

public class InventoryDbContext : DbContext , IInventoryDbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) {}
    
    public DbSet<StockTransaction> StockTransactions { get; set; }
    public DbSet<ProductView> ProductViews { get; set; }
    public DbSet<LocationView> LocationViews { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ProductView>(eb =>
        {
            eb.ToView("vw_ValidProducts");
            eb.HasNoKey();
        });

        builder.Entity<LocationView>(eb =>
        {
            eb.ToView("vw_ValidLocations");
            eb.HasNoKey();
        });

    }
}
