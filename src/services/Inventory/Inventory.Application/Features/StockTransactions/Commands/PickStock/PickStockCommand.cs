using MediatR;

namespace Inventory.Application.Features.StockTransactions.Commands.PickStock;

public class PickStockCommand : IRequest<bool>
{
    public Guid ProductId { get; set; }

    public Guid SourceLocationId { get; set; }

    public decimal QuantityToPick { get; set; }

    public string SourceDocument { get; set; } = string.Empty;

}