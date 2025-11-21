using Catalog.Application.Features.Dashboard.Queries.GetCatalogStats;
using MediatR;
using Catalog.Application.Features.Dashboard.Queries.GetCatalogStats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/catalog/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var query = new GetCatalogStatsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}