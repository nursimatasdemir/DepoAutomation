using MediatR;

namespace Job.Application.Features.Jobs.Commands.CompleteJob;

public class CompleteJobCommand : IRequest<bool>
{
    public Guid JobId { get; set; }
}