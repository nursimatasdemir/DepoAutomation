using Job.Domain.Enums;

namespace Job.Domain.Entities;

public class Job
{
    public Guid Id { get; set; }
    
    public JobType Type { get; set; }
    public JobStatus Status { get; set; } = JobStatus.Bekliyor;
    
    public string? SourceDocument { get; set; }
    
    public Guid? AssignedOperatorId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? FinishedAt { get; set; }

    public List<JobItem> Items { get; set; } = new();

}