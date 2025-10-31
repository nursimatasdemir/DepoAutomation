using MediatR;
using Microsoft.AspNetCore.Mvc;
using Catalog.Application.Features.Categories.Commands.CreateCategory;
using Catalog.Application.Features.Categories.Commands.DeleteCategory;
using Catalog.Application.Features.Categories.Queries.GetQueries;
using Catalog.Application.Features.Categories.Commands.UpdateCategory;

using Microsoft.EntityFrameworkCore;

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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryCommand command)
    {
        command.Id = id;
        
        var updatedCategoryDto = await _mediator.Send(command);
        if (updatedCategoryDto == null)
        {
            return NotFound();
        }
        return Ok(updatedCategoryDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id, [FromBody] DeleteCategoryCommand command)
    {
        command.Id = id;
        
        var wasdeleted = await _mediator.Send(command);
        if (!wasdeleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}