using Catalog.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler :IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryToDelete = await _context.Categories
            .FindAsync(request.Id);
        if (categoryToDelete == null)
        {
            return false;
        }

        _context.Categories.Remove(categoryToDelete);
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}