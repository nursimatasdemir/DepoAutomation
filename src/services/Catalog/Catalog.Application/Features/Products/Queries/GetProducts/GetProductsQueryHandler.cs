using Catalog.Application.Abstractions;
using Catalog.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetProductsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        if (!request.IncludeArchived)
        {
            query = query.Where(p => p.IsActive);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(p=>
                p.Name.ToLower().Contains(term) || 
                p.Sku.ToLower().Contains(term) ||
                (p.Barcode != null && p.Barcode.Contains(term)));
        }
        
        var products = await query
            .OrderBy(p => p.Name)
            .Select(p => new ProductDTO
            {
                Id = p.Id,
                Sku = p.Sku ?? string.Empty,
                Name = p.Name ?? string.Empty,
                Barcode = p.Barcode ?? string.Empty,
                CategoryName = p.Category != null ? p.Category.Name ??  string.Empty : string.Empty,
                IsActive = p.IsActive,
            })
            .ToListAsync(cancellationToken);
        return products;
    }
    

}