namespace Inventory.Application.DTOs;

public class StockTransactionDTO
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid LocationId { get; set; }
    public decimal QuantityChange { get; set; }
    public string SourceDocument { get; set; }
}