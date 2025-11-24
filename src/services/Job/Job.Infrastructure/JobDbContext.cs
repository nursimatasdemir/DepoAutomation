using Job.Application.Abstractions;
using Job.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Job.Infrastructure;

public class JobDbContext : DbContext, IJobDbContext
{
    public JobDbContext(DbContextOptions<JobDbContext> options) : base(options)
    {
    }
    
    public DbSet<Job.Domain.Entities.Job> Jobs { get; set; }
    public DbSet<JobItem> JobItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Job.Domain.Entities.Job>()
            .HasMany(j => j.Items)
            .WithOne()
            .HasForeignKey(j => j.JobId)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);

    }
    
}