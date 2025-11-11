using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace Identity.Application.Features.Auth.Commands.Register;
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    
    public RegisterCommandHandler(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userManager.FindByNameAsync(request.Username);
        if (userExists != null)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("Username", $" Bu kullanıcı adı ({request.Username}) kullanılıyor.")
            });
        }
        
        var roleExists = await _roleManager.FindByNameAsync(request.Role);
        if (roleExists == null)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("RoleName", $"Bu rol ({request.Role}) sistemde mevcut değil!")
            });
        }

        var newUser = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = request.Username,
        };
        
        var result = await _userManager.CreateAsync(newUser, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new ValidationFailure(e.Code, e.Description));
            throw new ValidationException(errors);
        }
        
        await _userManager.AddToRoleAsync(newUser, request.Role);
        return true;
    }
}