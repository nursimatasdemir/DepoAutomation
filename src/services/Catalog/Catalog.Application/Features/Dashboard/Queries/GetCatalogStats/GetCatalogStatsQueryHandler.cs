using Catalog.Application.Abstractions;
using Catalog.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Dashboard.Queries.GetCatalogStats;

public class GetCatalogStatsQueryHandler : IRequestHandler<GetCatalogStatsQuery, CatalogStatsDTO>
{
    private readonly IApplicationDbContext _context;

    public GetCatalogStatsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CatalogStatsDTO> Handle(GetCatalogStatsQuery request, CancellationToken cancellationToken)
    {
        var productCountTask = await _context.Products.CountAsync(cancellationToken);
        var activeProductCountTask = await _context.Products.CountAsync(p => p.IsActive, cancellationToken);
        var categoriesCountTask = await _context.Categories.CountAsync(cancellationToken);
        var locationCountTask = await _context.Locations.CountAsync(cancellationToken);
        
        return new CatalogStatsDTO
        {
            ProductCount = productCountTask,
            ActiveProductCount = activeProductCountTask,
            CategoryCount = categoriesCountTask,
            LocationCount = locationCountTask,
        };
    }
}