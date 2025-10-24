using MediatR;
using Catalog.Application.Features.Products.Queries.GetProducts;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var query = new GetProductsQuery();
        
        var products = await _mediator.Send(query);
        return Ok(products);
    }
}