using System.ComponentModel.DataAnnotations;
using Catalog.Application.Abstractions;
using Catalog.Domain;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace Catalog.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Name.ToLower() == request.Name.ToLower(), cancellationToken);
        
        if (!categoryExists)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("Category", $"Bu ad ile kayıtlı kategori mevcut : {request.Name}")
            });
        }
        
        
        var newCategory = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
        };
        
        await _context.Categories.AddAsync(newCategory, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return newCategory.Id;
    }
}