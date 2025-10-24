using Catalog.Application.Abstractions;
using Catalog.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Categories.Queries.GetQueries;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetCategoriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryDTO>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories
            .Include(c => c.Products)
            .Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name ?? string.Empty,
                ProductCount = c.Products.Count
            })
            .ToListAsync(cancellationToken);
        return categories;
    }
}