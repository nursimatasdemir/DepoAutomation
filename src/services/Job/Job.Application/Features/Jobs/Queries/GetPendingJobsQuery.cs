using Job.Application.DTOs;
using MediatR;

namespace Job.Application.Features.Jobs.Queries;

public class GetPendingJobsQuery : IRequest<List<JobDTO>>
{
    
}