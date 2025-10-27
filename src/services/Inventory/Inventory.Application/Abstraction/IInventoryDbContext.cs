using Inventory.Domain;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Abstraction;

public interface IInventoryDbContext
{
    DbSet<StockTransaction> StockTransactions { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
