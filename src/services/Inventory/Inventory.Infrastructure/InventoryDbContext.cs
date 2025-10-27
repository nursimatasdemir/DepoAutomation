using Microsoft.EntityFrameworkCore;
using Inventory.Domain;

namespace Inventory.Infrastructure;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
    {
    }
    
    public DbSet<StockTransaction> StockTransactions { get; set; }
}