using Catalog.Application.Features.Products.Commands.CreateProduct;
using Catalog.Application.Features.Products.Commands.UpdateProduct;
using Catalog.Application.Features.Products.Queries.GetProductById;
using MediatR;
using Catalog.Application.Features.Products.Queries.GetProducts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Operator")]
    public async Task<IActionResult> GetProducts()
    {
        var query = new GetProductsQuery();
        var products = await _mediator.Send(query);
        return Ok(products);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Operator")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var query = new GetProductByIdQuery { Id = id };
        var product = await _mediator.Send(query);
        if(product == null)
            return NotFound();
        return Ok(product);
        
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        var newProductId = await _mediator.Send(command);
        return Ok(newProductId);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductCommand command)
    {
        command.Id = id;
        
        var updatedProductDto = await _mediator.Send(command);
        if (updatedProductDto == null)
        {
            return NotFound();
        }
        return Ok(updatedProductDto);
    }
    
}