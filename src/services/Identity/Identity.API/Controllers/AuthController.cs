using Identity.Application.Features.Auth.Commands.Login;
using Identity.Application.Features.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand registerCommand)
    {
        var wasSuccessful = await _mediator.Send(registerCommand);

        if (!wasSuccessful)
        {
            return BadRequest("Kayıt işlemi başarısız!");
        }

        return Ok("Kullanıcı başarıyla oluşturuldu");
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand loginCommand)
    {
        var tokenString = await _mediator.Send(loginCommand);
        return Ok(new { token = tokenString });
    }
    
}