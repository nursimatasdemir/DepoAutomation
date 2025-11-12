using Catalog.Application.Abstractions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Catalog.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler :IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<DeleteCategoryCommand> _logger;

    public DeleteCategoryCommandHandler(IApplicationDbContext context, IConnectionMultiplexer redis, ILogger<DeleteCategoryCommand> logger)
    {
        _context = context;
        _redis = redis;
        _logger = logger;
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
        
        var hasActiveProducts = await _context.Products.
            AnyAsync(p=>p.CategoryId==request.Id && p.IsActive == true, cancellationToken);

        if (hasActiveProducts)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("Category",
                    $"Bu kategori ({categoryToDelete.Name}) silinemez, çünkü hala bu kategoriye ait aktif ürün bulunuyor. ")
            });
        }

        _context.Categories.Remove(categoryToDelete);
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}