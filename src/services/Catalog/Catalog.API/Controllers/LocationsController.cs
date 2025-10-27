using MediatR;
using Microsoft.AspNetCore.Mvc;
using Catalog.Application.Features.Locations.Commands.CreateLocation;
using Catalog.Application.Features.Locations.Commands.UpdateLocation;
using Catalog.Application.Features.Locations.Queries.GetLocations;

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

    [HttpGet]
    public async Task<IActionResult> GetLocations()
    {
        var query = new GetLocationsQuery();
        var locations = await _mediator.Send(query);
        return Ok(locations);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLocation([FromBody] CreateLocationCommand command)
    {
        var newLocationId = await _mediator.Send(command);
        return Ok(newLocationId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLocation([FromRoute] Guid id, [FromBody] UpdateLocationCommand command)
    {
        command.Id = id;
        
        var updatedLocation = await _mediator.Send(command);
        if(updatedLocation == null) return NotFound();
        
        return Ok(updatedLocation);
    }
}