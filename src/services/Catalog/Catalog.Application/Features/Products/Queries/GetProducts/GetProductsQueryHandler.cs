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
        var products = await _context.Products
            .Include(p => p.Category)
            .Where(p=>p.IsActive == true)
            .Select(p => new ProductDTO
            {
                Id = p.Id,
                Sku = p.Sku ?? String.Empty,
                Name = p.Name ?? String.Empty,
                Barcode = p.Barcode ?? String.Empty,
                CategoryName = p.Category != null ? p.Category.Name ?? string.Empty : string.Empty
            })
            .ToListAsync(cancellationToken);
        return products;
    }
    
}