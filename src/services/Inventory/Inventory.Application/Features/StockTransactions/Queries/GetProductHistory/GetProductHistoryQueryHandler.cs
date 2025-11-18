using Inventory.Application.Abstraction;
using Inventory.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.StockTransactions.Queries.GetProductHistory;

public class GetProductHistoryQueryHandler : IRequestHandler<GetProductHistoryQuery, List<StockTransactionDTO>>
{
    private readonly IInventoryDbContext _context;

    public GetProductHistoryQueryHandler(IInventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<StockTransactionDTO>> Handle(GetProductHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var history = await _context.StockTransactions
            .Where(st => st.ProductId == request.ProductId)
            .OrderByDescending(st => st.Timestamp)
            .Select(st => new StockTransactionDTO
            {
                Id = st.Id,
                Timestamp = st.Timestamp,
                LocationId = st.LocationId,
                QuantityChange = st.QuantityChange,
                SourceDocument = st.SourceDocument,
            })
            .ToListAsync(cancellationToken);
        return history;
    }
}