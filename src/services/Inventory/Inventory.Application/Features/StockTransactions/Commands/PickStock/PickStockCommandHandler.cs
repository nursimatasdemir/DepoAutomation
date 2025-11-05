using System.Data.Common;
using FluentValidation;
using FluentValidation.Results;
using Inventory.Application.Abstraction;
using Inventory.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;


namespace Inventory.Application.Features.StockTransactions.Commands.PickStock;

public class PickStockCommandHandler : IRequestHandler<PickStockCommand, bool>
{
    private readonly IInventoryDbContext _context;
    private readonly IConnectionMultiplexer _redis;

    public PickStockCommandHandler(IInventoryDbContext context, IConnectionMultiplexer redis)
    {
        _context = context;
        _redis = redis;
    }

    public async Task<bool> Handle(PickStockCommand request, CancellationToken cancellationToken)
    {
        var productExists = await _context.ProductViews
            .AnyAsync(p => p.Id == request.ProductId, cancellationToken);
        if (!productExists)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("ProductId", $"(...) geçerli bir ürün Id numarası değil.")
            });
        }
        
        var sourceLocationExists = await _context.LocationViews
            .AnyAsync(l=>l.Id == request.SourceLocationId, cancellationToken);

        if (!sourceLocationExists)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("SourceLocationId",
                    $"(...) geçerli bir kaynak lokasyona ait Id numarası değil.")
            });
        }
        
        
        var redisDb = _redis.GetDatabase();
        string sourceKey = $"stock:{request.ProductId}:{request.SourceLocationId}";
        
        var sourceStockValue = await redisDb.StringGetAsync(sourceKey);

        if (!sourceStockValue.TryParse(out double currentSourceStock) ||
            currentSourceStock < (double)request.QuantityToPick)
        {
            return false;
        }

        await using var dbTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var transactionOut = new StockTransaction
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                ProductId = request.ProductId,
                LocationId = request.SourceLocationId,
                QuantityChange = -request.QuantityToPick,
                SourceDocument = request.SourceDocument
            };
            
            await _context.StockTransactions.AddAsync(transactionOut);
            await _context.SaveChangesAsync(cancellationToken);
            
            await redisDb.StringDecrementAsync(sourceKey, (double)request.QuantityToPick);

            await dbTransaction.CommitAsync(cancellationToken);
            
            return true;

        }
        catch (Exception ex)
        {
            await dbTransaction.RollbackAsync(cancellationToken);
            return false;
        }
    }
}