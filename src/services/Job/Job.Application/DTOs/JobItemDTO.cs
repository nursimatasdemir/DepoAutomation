using Job.Domain.Enums;
namespace Job.Application.DTOs;

public class JobItemDTO
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public Guid? SourceLocationId { get; set; }
    public Guid? TargetLocationId { get; set; }
    public decimal Quantity { get; set; }
}