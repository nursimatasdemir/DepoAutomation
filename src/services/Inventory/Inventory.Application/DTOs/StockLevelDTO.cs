namespace Inventory.Application.DTOs;

public class StockLevelDTO
{
    public Guid ProductId { get; set; }

    public decimal TotalQuantity { get; set; }
}