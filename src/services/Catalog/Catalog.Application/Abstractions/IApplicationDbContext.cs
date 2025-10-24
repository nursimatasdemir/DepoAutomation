using Catalog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<Location> Locations { get; set; }

    Task<int>SaveChangesAsync(CancellationToken cancellationToken) ;
}