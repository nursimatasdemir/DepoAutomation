using Job.Application.Abstractions;
using Job.Application.DTOs;
using Job.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Job.Application.Features.Jobs.Queries;

public class GetPendingJobsQueryHandler : IRequestHandler<GetPendingJobsQuery, List<JobDTO>>
{
    private readonly IJobDbContext _context;

    public GetPendingJobsQueryHandler(IJobDbContext context)
    {
        _context = context;
    }

    public async Task<List<JobDTO>> Handle(GetPendingJobsQuery request, CancellationToken cancellationToken)
    {
        var jobs = await _context.Jobs
            .Include(j=>j.Items)
            .Where(j=>j.Status == JobStatus.Bekliyor)
            .OrderByDescending(j=>j.CreatedAt)
            .ToListAsync(cancellationToken);
        
        var jobDTOs = jobs.Select(j => new JobDTO
        {
            Id = j.Id,
            Type = j.Type.ToString(),
            Status = j.Status.ToString(),
            SourceDocument = j.SourceDocument,
            CreatedAt = j.CreatedAt,
            Items = j.Items.Select(i => new JobItemDTO
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    SourceLocationId = i.SourceLocationId,
                    TargetLocationId = i.TargetLocationId,
                }).ToList()
            }).ToList();
        return jobDTOs;
    }
}