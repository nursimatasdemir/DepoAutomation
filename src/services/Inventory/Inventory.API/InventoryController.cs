using MediatR;
using Microsoft.AspNetCore.Mvc;
using Inventory.Application.Features.StockTransactions.Queries.GetStockLevel;
using Inventory.Application.DTOs;
using Inventory.Application.Features.StockTransactions.Commands.PickStock;
using Inventory.Application.Features.StockTransactions.Commands.TransferStock;

using Inventory.Application.Features.StockTransactions.Commands.ReceiveStock;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.API;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("receive")]
    [Authorize(Roles = "Admin, Operator")]
    public async Task<IActionResult> ReceiveStock([FromBody] ReceiveStockCommand command)
    {
        var transactionId = await _mediator.Send(command);
        return Ok(transactionId);
    }

    [HttpPost("transfer")]
    [Authorize(Roles = "Admin, Operator")]
    public async Task<IActionResult> TransferStock([FromBody] TransferStockCommand command)
    {
        var wasSuccessful = await _mediator.Send(command);
        if (!wasSuccessful)
        {
            return BadRequest("İşlem başarısız kaynak lokasyonda yeteri kadar stok olmayabilir");
        }

        return NoContent();
    }

    [HttpPost("pick")]
    [Authorize(Roles = "Admin, Operator")]
    public async Task<IActionResult> PickStock([FromBody] PickStockCommand command)
    {
        var wasSuccessful = await _mediator.Send(command);
        if (!wasSuccessful)
        {
            return BadRequest("İşlem başarısız kaynak lokasyonda yeteri kadar stok olmayabilir");
        }

        return NoContent();
    }

    [HttpGet("stock/{productId}")]
    [Authorize(Roles = "Admin, Operator")]
    public async Task<IActionResult> GetStockLevel([FromRoute] Guid productId)
    {
        var query = new GetStockLevelQuery { ProductId = productId };
        var stockLevel = await _mediator.Send(query);
        
        return Ok(stockLevel);
    }
}