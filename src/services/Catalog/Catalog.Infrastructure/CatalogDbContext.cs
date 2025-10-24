using System.Net.Mime;
using Catalog.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Catalog.Domain;

namespace Catalog.Infrastructure;

public class CatalogDbContext : DbContext, IApplicationDbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Location> Locations { get; set; }
}