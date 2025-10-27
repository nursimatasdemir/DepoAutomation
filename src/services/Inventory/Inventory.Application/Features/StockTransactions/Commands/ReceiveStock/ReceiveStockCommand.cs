using MediatR;

namespace Inventory.Application.Features.StockTransactions.Commands.ReceiveStock;

public class ReceiveStockCommand : IRequest<Guid>
{
    public Guid ProductId{ get; set; }
    public decimal QuantityReceived { get; set; }
    public Guid LocationId { get; set; }
    public string SourceDocument { get; set; } = string.Empty;
}