using MediatR;

namespace Identity.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<string>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}