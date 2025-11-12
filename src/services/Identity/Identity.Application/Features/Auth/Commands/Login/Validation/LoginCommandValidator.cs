using FluentValidation;

namespace Identity.Application.Features.Auth.Commands.Login.Validation;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Kullanıcı adı boş bırakılamaz!");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre boş bırakılamaz!")
            .MinimumLength(4).WithMessage("Şifre en az 4 karakter olmalıdır.");

    }
}