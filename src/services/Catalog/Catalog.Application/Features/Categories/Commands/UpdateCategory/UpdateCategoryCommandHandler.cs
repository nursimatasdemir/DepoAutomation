using Catalog.Application.Abstractions;
using Catalog.Application.DTOs;
using Catalog.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDTO?>
{
    private readonly IApplicationDbContext _context;

    public UpdateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryDTO?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryToUpdate = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (categoryToUpdate == null)
        {
            return null;
        }
        
        categoryToUpdate.Name = request.Name;
        await _context.SaveChangesAsync(cancellationToken);

        return new CategoryDTO
        {
            Id = categoryToUpdate.Id,
            Name = categoryToUpdate.Name ?? string.Empty,
            ProductCount = categoryToUpdate.Products.Count
        };
    }
}