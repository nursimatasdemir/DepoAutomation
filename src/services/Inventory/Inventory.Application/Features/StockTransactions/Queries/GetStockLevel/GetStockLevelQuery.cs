using Inventory.Application.DTOs;
using MediatR;

namespace Inventory.Application.Features.StockTransactions.Queries.GetStockLevel;

public class GetStockLevelQuery : IRequest<StockLevelDTO>
{
    public Guid ProductId { get; set; }
}