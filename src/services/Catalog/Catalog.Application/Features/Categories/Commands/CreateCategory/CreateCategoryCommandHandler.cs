using Catalog.Application.Abstractions;
using Catalog.Domain;
using MediatR;

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