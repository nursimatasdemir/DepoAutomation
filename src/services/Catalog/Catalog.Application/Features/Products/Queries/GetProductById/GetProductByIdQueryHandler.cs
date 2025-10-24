using Catalog.Application.Abstractions;
using Catalog.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDTO?>
{
    private readonly IApplicationDbContext _context;

    public GetProductByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDTO?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Select(p=> new ProductDTO
            {
                Id = p.Id,
                Sku = p.Sku ?? string.Empty,
                Name = p.Name ?? string.Empty,
                Barcode = p.Barcode ?? string.Empty,
                CategoryName = p.Category != null ? p.Category.Name ?? string.Empty : string.Empty,
            })
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        return product;
    }
}