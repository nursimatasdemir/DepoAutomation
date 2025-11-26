using Job.Application.Features.Jobs.Commands.CompleteJob;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Job.Application.Features.Jobs.Commands.CreateJob;
using Job.Application.Features.Jobs.Queries;
using Microsoft.AspNetCore.Authorization;

namespace Job.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobsController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("pending")]
    [Authorize(Roles = "Admin, Operator")]
    public async Task<IActionResult> GetPendingJobs()
    {
        var query = new GetPendingJobsQuery();
        var jobs = await _mediator.Send(query);
        return Ok(jobs);
    }
    
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobCommand command)
    {
        var jobId = await _mediator.Send(command);
        return Ok(jobId);
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteJob([FromRoute] Guid id)
    {
        var command = new CompleteJobCommand {JobId = id};
        var success = await _mediator.Send(command);

        if (!success)
        {
            return NotFound("İş bulunamadı.");
        }

        return NoContent();
    }
    
}