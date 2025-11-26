using Job.Domain.Enums;
namespace Job.Application.DTOs;

public class JobDTO
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? SourceDocument { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public List<JobItemDTO> Items { get; set; } = new();
}