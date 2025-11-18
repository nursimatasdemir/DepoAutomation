using Inventory.Application.DTOs;
using MediatR;

namespace Inventory.Application.Features.StockTransactions.Queries.GetProductHistory;

public class GetProductHistoryQuery : IRequest<List<StockTransactionDTO>>
{
    public Guid ProductId { get; set; }
}