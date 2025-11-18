using MediatR;
using Microsoft.AspNetCore.Mvc;
using Catalog.Application.Features.Categories.Commands.CreateCategory;
using Catalog.Application.Features.Categories.Commands.DeleteCategory;
using Catalog.Application.Features.Categories.Queries.GetQueries;
using Catalog.Application.Features.Categories.Commands.UpdateCategory;
using Catalog.Application.Features.Categories.Queries.GetCategoriesById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Operator")]
    public async Task<IActionResult> GetCategories()
    {
        var query = new GetCategoriesQuery();
        var categories = await _mediator.Send(query);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Operator")]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
    {
        var query = new GetCategoryByIdQuery { Id = id };
        var category = await _mediator.Send(query);
        if(category == null) return NotFound();
        return Ok(category);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
    {
        var newCategoryId = await _mediator.Send(command);
        return Ok(newCategoryId);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
    {
        var command = new DeleteCategoryCommand { Id = id };
        var wasDeleted = await _mediator.Send(command);
        if (!wasDeleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}