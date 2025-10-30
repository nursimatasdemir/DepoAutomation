using Inventory.Application.Abstraction;
using Inventory.Domain;
using MediatR;
using StackExchange.Redis;
using System;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.StockTransactions.Commands.TransferStock;

public class TransferStockCommandHandler : IRequestHandler<TransferStockCommand, bool>
{
    private readonly IInventoryDbContext _context;
    private readonly IConnectionMultiplexer _redis;

    public TransferStockCommandHandler(IInventoryDbContext context, IConnectionMultiplexer redis)
    {
        _context = context;
        _redis = redis;
    }

    public async Task<bool> Handle(TransferStockCommand request, CancellationToken cancellationToken)
    {
        var redisDb = _redis.GetDatabase();
        string sourceKey = $"stock:{request.ProductId}:{request.SourceLocationId}";
        
        var sourceStockValue = await redisDb.StringGetAsync(sourceKey);

        if (!sourceStockValue.TryParse(out double currentSourceStock) ||
            currentSourceStock < (double)request.QuantityToTransfer)
        {
            return false;
        }
        
        await using var dbTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var transactionTimeStamp = DateTime.UtcNow;

            var transactionOut = new StockTransaction
            {
                Id = Guid.NewGuid(),
                Timestamp = transactionTimeStamp,
                ProductId = request.ProductId,
                LocationId = request.SourceLocationId,
                QuantityChange = -request.QuantityToTransfer,
                SourceDocument = request.Reason
            };

            var transactionIn = new StockTransaction
            {
                Id = Guid.NewGuid(),
                Timestamp = transactionTimeStamp,
                ProductId = request.ProductId,
                LocationId = request.DestinationLocationId,
                QuantityChange = request.QuantityToTransfer,
                SourceDocument = request.Reason
            };
            
            await _context.StockTransactions.AddRangeAsync(transactionOut, transactionIn);
            await _context.SaveChangesAsync(cancellationToken);
            
            string destinationKey = $"stock:{request.ProductId}:{request.DestinationLocationId}";
            
            await redisDb.StringDecrementAsync(sourceKey, (double)request.QuantityToTransfer);
            await redisDb.StringIncrementAsync(destinationKey, (double)request.QuantityToTransfer);
            
            await dbTransaction.CommitAsync(cancellationToken);
            
            return true;
        }
        catch (Exception e)
        {
            await dbTransaction.RollbackAsync(cancellationToken);
            return false;
        }
    }
}