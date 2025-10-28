using Inventory.Application.Abstraction;
using Inventory.Domain;
using MediatR;
using System;
using StackExchange.Redis;

namespace Inventory.Application.Features.StockTransactions.Commands.ReceiveStock;

public class ReceiveStockCommandHandler : IRequestHandler<ReceiveStockCommand, Guid>
{
    private readonly IInventoryDbContext _context;
    private readonly IConnectionMultiplexer _redis;

    public ReceiveStockCommandHandler(IInventoryDbContext context, IConnectionMultiplexer redis)
    {
        _context = context;
        _redis = redis;
    }

    public async Task<Guid> Handle(ReceiveStockCommand request, CancellationToken cancellationToken)
    {
        var transaction = new StockTransaction
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            ProductId = request.ProductId,
            LocationId = request.LocationId,
            QuantityChange = request.QuantityReceived,
            SourceDocument = request.SourceDocument
        };
        await _context.StockTransactions.AddAsync(transaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        var redisDb = _redis.GetDatabase();
        string redisKey = $"stock:{request.ProductId}:{request.LocationId}";

        await redisDb.StringIncrementAsync(redisKey, (double)request.QuantityReceived);
        
        return transaction.Id;
    }
}