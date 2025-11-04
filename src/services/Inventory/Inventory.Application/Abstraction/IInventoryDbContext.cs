using Inventory.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Inventory.Domain.Views;

namespace Inventory.Application.Abstraction;

public interface IInventoryDbContext
{
    DbSet<StockTransaction> StockTransactions { get; }
    DbSet<ProductView> ProductViews { get; }
    DbSet<LocationView> LocationViews { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
    DatabaseFacade Database { get; }
}
