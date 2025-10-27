using MediatR;
using Microsoft.AspNetCore.Mvc;

using Inventory.Application.Features.StockTransactions.Commands.ReceiveStock;

namespace Inventory.API;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("receive")]
    public async Task<IActionResult> ReceiveStock([FromBody] ReceiveStockCommand command)
    {
        var transactionId = await _mediator.Send(command);
        return Ok(transactionId);
    }
}