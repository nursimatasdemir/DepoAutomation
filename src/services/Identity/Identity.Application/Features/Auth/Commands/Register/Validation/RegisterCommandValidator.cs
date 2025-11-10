using FluentValidation;
namespace Identity.Application.Features.Auth.Commands.Register.Validation;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Kullanıcı adı boş bırakılamaz!");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre boş bırakılamaz!")
            .MinimumLength(4).WithMessage("Şifre en az 4 karakter olmalıdır.");
        RuleFor(x => x.Role).NotEmpty().WithMessage("Kullanıcı rolü tanımlayınız!");
    }
}