using Catalog.Application.Abstractions;
using Catalog.Application.DTOs;
using Catalog.Domain;
using FluentValidation;
using FluentValidation.Results;
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
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.Id, cancellationToken);
        if (!categoryExists)
        {
            throw new FluentValidation.ValidationException("Verilen Id ile kayıtlı kategori bulunamadı.");
        }
        var nameExists = await _context.Categories.AnyAsync(c => c.Name == request.Name, cancellationToken);
        if (nameExists)
        {
            throw new FluentValidation.ValidationException("Mevcut olan adlardan birini kullanamazsınız.");
        }
        
        
        var categoryToUpdate = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (categoryToUpdate == null)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("CategoryId", $"{request.Id} numarası ile kayıtlı kategori bulunamadı.")
            });
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