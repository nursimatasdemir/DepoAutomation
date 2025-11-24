using Job.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace Job.Application.Abstractions;

public interface IJobDbContext
{
    DbSet<Job.Domain.Entities.Job> Jobs { get; }
    DbSet<JobItem> JobItems { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DatabaseFacade Database { get; }
}