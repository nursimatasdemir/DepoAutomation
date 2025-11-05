using Catalog.Application.Abstractions;
using FluentValidation.Results;
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
        var categoryToDelete = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (categoryToDelete == null)
        {
            throw new FluentValidation.ValidationException(new[]
            {
                new ValidationFailure("Category",
                    $"Verilen {request.Id} Id numaralı kategori kayıtına ulaşılamadı. Id numarasını veya kategorinin varlığını kontrol edin.")
            });
        }

        _context.Categories.Remove(categoryToDelete);
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}