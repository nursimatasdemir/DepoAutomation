namespace Job.Application.DTOs;

public class CreateJobItemDTO
{
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    
    public Guid? SourceLocationId { get; set; }
    public Guid? TargetLocationId { get; set; }
}