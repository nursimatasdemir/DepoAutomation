using Job.Application.Abstractions;
using Job.Domain.Entities;
using Job.Domain.Enums;
using MediatR;

namespace Job.Application.Features.Jobs.Commands.CreateJob;

public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, Guid>
{
    private readonly IJobDbContext _context;

    public CreateJobCommandHandler(IJobDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var newJob = new Domain.Entities.Job
        {
            Id = Guid.NewGuid(),
            Type = request.Type,
            Status = JobStatus.Bekliyor,
            SourceDocument = request.SourceDocument,
            CreatedAt = DateTime.UtcNow,
        };

        if (request.Items != null && request.Items.Any())
        {
            foreach (var itemDTO in request.Items)
            {
                var jobItem = new JobItem
                {
                    Id = Guid.NewGuid(),
                    JobId = newJob.Id,
                    ProductId = itemDTO.ProductId,
                    Quantity = itemDTO.Quantity,
                    SourceLocationId = itemDTO.SourceLocationId,
                    TargetLocationId = itemDTO.TargetLocationId,
                };
                newJob.Items.Add(jobItem);
            }
        }
        
        await _context.Jobs.AddAsync(newJob, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return newJob.Id;
    }   
    
}
