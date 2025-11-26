using Job.Application.Abstractions;
using Job.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Job.Application.Features.Jobs.Commands.CompleteJob;

public class CompleteJobCommandHandler : IRequestHandler<CompleteJobCommand, bool>
{
    private readonly IJobDbContext _context;

    public CompleteJobCommandHandler(IJobDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CompleteJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _context.Jobs
            .FirstOrDefaultAsync(j => j.Id == request.JobId, cancellationToken);

        if (job == null)
        {
            return false;
        }

        if (job.Status == JobStatus.Tamamlandı)
        {
            return true;
        }

        job.Status = JobStatus.Tamamlandı;
        job.FinishedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}