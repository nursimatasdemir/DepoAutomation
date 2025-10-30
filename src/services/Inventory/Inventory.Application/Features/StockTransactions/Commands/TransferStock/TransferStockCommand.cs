using MediatR;

namespace Inventory.Application.Features.StockTransactions.Commands.TransferStock;

public class TransferStockCommand : IRequest<bool>
{
    public Guid ProductId { get; set; }
    
    public Guid SourceLocationId { get; set; }
    
    public Guid DestinationLocationId { get; set; }
    
    public decimal QuantityToTransfer { get; set; }
    
    public string Reason { get; set; } = string.Empty;
    
    
}