using MediatR;
namespace Identity.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<bool>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}