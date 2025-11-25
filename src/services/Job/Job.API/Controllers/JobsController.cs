using MediatR;
using Microsoft.AspNetCore.Mvc;
using Job.Application.Features.Jobs.Commands.CreateJob;
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

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobCommand command)
    {
        var jobId = await _mediator.Send(command);
        return Ok(jobId);
    }
}