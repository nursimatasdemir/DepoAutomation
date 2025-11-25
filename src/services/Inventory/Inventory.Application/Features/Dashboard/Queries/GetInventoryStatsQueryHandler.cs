using Inventory.Application.Abstraction;
using Inventory.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Dashboard.Queries;

public class GetInventoryStatsQueryHandler : IRequestHandler<GetInventoryStatsQuery, InventoryStatsDTO>
{
    private readonly IInventoryDbContext _context;

    public GetInventoryStatsQueryHandler(IInventoryDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryStatsDTO> Handle(GetInventoryStatsQuery request, CancellationToken cancellationToken)
    {
        var totalTx = await _context.StockTransactions.CountAsync(cancellationToken);
        var incoming = await _context.StockTransactions.CountAsync(t => t.QuantityChange > 0 ,cancellationToken);
        var outgoing = await _context.StockTransactions.CountAsync(t => t.QuantityChange < 0 ,cancellationToken);
        
        var totalStock = await _context.StockTransactions.SumAsync(t => t.QuantityChange,cancellationToken);

        
        return new InventoryStatsDTO
        {
            TotalTransactions = totalTx,
            IncomiingTransactions = incoming,
            OutgoingTransactions = outgoing,
            TotalItemsInStock = totalStock
        };
    }
}