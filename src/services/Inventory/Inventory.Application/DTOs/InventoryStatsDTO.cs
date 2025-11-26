namespace Inventory.Application.DTOs;

public class InventoryStatsDTO
{
    public int TotalTransactions { get; set; }
    public int IncomingTransactions { get; set; }
    public int OutgoingTransactions { get; set; }
    public decimal TotalItemsInStock { get; set; }
}