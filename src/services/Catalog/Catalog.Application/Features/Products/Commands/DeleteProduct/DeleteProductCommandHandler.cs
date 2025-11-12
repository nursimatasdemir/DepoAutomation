using System.ComponentModel.DataAnnotations;
using Catalog.Application.Abstractions;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Catalog.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<DeleteProductCommand> _logger;

    public DeleteProductCommandHandler(IApplicationDbContext context, IConnectionMultiplexer redis, ILogger<DeleteProductCommand> logger)
    {
        _context = context;
        _redis = redis;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var productToDelete = await _context.Products.FirstOrDefaultAsync(c=>c.Id == request.Id, cancellationToken);

        if (productToDelete == null)
        {
            throw new FluentValidation.ValidationException(new[]
            {
                new ValidationFailure("Product",
                    $"Verilen {request.Id} Id numaralı ürün kayıtına ulaşılamadı. Id numarasını veya ürün varlığını kontrol edin.")
            });
        }
        
        var redisDb = _redis.GetDatabase();
        var server = _redis.GetServer("localhost", 6379);
        var pattern = $"stock:{request.Id}:*";
        var keys = server.Keys(pattern: pattern);
        
        decimal totalStock = 0;
        foreach (var key in keys)
        {
            var valueString = await redisDb.StringGetAsync(key);
            if (valueString.TryParse(out double quantity))
            {
                totalStock += (decimal)quantity;
            }
        }

        if (totalStock > 0)
        {
            throw new FluentValidation.ValidationException(new[]
            {
                new ValidationFailure("Stock",
                    $"Bu ürün ({productToDelete.Name}) silinemez çünkü depoda hala {totalStock} adet stoğu bulunmaktadır.")
            });
        }
        
        productToDelete.IsActive = false;
        
        await _context.SaveChangesAsync(cancellationToken);
        try
        {
            await redisDb.SetRemoveAsync("valid_products", request.Id.ToString());
            var keysToDelete = server.Keys(pattern: pattern).ToArray();

            if (keysToDelete.Length > 0)
            {
                await redisDb.KeyDeleteAsync(keysToDelete);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ürün arşivlendi ANCAK Redis cache temizlenemedi.");
            throw;
        }
        
        return true;
    }
}