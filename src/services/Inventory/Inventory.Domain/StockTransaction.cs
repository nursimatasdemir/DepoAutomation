namespace Inventory.Domain;

public class StockTransaction
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid ProductId { get; set; }
    public Guid LocationId { get; set; }
    public decimal QuantityChange { get; set; }
    public string SourceDocument { get; set; }
    
}