using Microsoft.EntityFrameworkCore;
using Inventory.Domain;
using Inventory.Application.Abstraction;

namespace Inventory.Infrastructure;

public class InventoryDbContext : DbContext , IInventoryDbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) {}
    
    public DbSet<StockTransaction> StockTransactions { get; set; }
}
