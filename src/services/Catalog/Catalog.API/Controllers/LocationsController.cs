using MediatR;
using Microsoft.AspNetCore.Mvc;
using Catalog.Application.Features.Locations.Commands.CreateLocation;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LocationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateLocation([FromBody] CreateLocationCommand command)
    {
        var newLocationId = await _mediator.Send(command);
        return Ok(newLocationId);
    }
}