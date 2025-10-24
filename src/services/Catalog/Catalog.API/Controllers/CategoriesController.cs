using MediatR;
using Microsoft.AspNetCore.Mvc;
using Catalog.Application.Features.Categories.Commands.CreateCategory;
using Catalog.Application.Features.Categories.Queries.GetQueries;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var query = new GetCategoriesQuery();
        var categories = await _mediator.Send(query);
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
    {
        var newCategoryId = await _mediator.Send(command);
        return Ok(newCategoryId);
    }
}