namespace Job.Domain.Entities;

public class JobItem
{
    public Guid Id { get; set; }
    
    public Guid JobId { get; set; }
    
    public Guid ProductId { get; set; }
    
    public Guid? SourceLocationId { get; set; }
    
    public Guid? TargetLocationId { get; set; }
    
    public decimal Quantity { get; set; }

    public bool IsCompleted { get; set; } = false;
} 