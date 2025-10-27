using Inventory.Application.Abstraction;
using Inventory.Domain;
using MediatR;
using System;

namespace Inventory.Application.Features.StockTransactions.Commands.ReceiveStock;

public class ReceiveStockCommandHandler : IRequestHandler<ReceiveStockCommand, Guid>
{
    private readonly IInventoryDbContext _context;

    public ReceiveStockCommandHandler(IInventoryDbContext context)
    {
        _context = context;
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
        
        return transaction.Id;
    }
}