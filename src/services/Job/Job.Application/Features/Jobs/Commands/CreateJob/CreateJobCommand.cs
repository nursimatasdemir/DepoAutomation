using Job.Domain.Entities;
using Job.Application.DTOs;
using Job.Domain.Enums;
using MediatR;

namespace Job.Application.Features.Jobs.Commands.CreateJob;

public class CreateJobCommand : IRequest<Guid>
{
    public JobType Type { get; set; }
    public string? SourceDocument { get; set; }
    public List<JobItemDTO>? Items { get; set; } = new();
    
}