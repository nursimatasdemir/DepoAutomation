using Inventory.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Inventory.Application.Abstraction;

public interface IInventoryDbContext
{
    DbSet<StockTransaction> StockTransactions { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
    DatabaseFacade Database { get; }
}
