using Identity.Application.Features.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand registerCommand)
    {
        var wasSuccessful = await _mediator.Send(registerCommand);

        if (!wasSuccessful)
        {
            return BadRequest("Kayıt işlemi başarısız!");
        }

        return Ok("Kullanıcı başarıyla oluşturuldu");
    }
    
}