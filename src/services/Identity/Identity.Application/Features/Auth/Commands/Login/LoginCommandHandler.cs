using FluentValidation;
using FluentValidation.Results;
using Identity.Application.Abstractions;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
namespace Identity.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    
    public LoginCommandHandler(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("Auth", $"Kullanıcı adı veya şifre hatalı.")
            });
        }
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("Auth", $"Kullanıcı adı veya şifre hatalı.")
            });
        }
        
        var roles = await _userManager.GetRolesAsync(user);
        
        var token = _jwtTokenGenerator.CreateToken(user, roles);
        return token;
    }
}