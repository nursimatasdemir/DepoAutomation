using FluentValidation;

namespace Catalog.Application.Features.Locations.Commands.UpdateLocation.Validation;

public class UpdateLocationCommandValidator : AbstractValidator<UpdateLocationCommand>
{
    public UpdateLocationCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty()
            .WithMessage("Güncellemek istediğiniz lokasyona ait Id bilgisi giriniz.");
        RuleFor(command => command.Code).NotEmpty().WithMessage("Lokasyon kodu boş bırakılamaz")
            .MinimumLength(3).WithMessage("Lokasyon Kodu en az 3 karakter içermelidir");
        RuleFor(command => command.Type).NotEmpty().WithMessage("Lokasyon bilgisini açıkça belirtiniz!")
            .MinimumLength(3).WithMessage("Lokasyon belirteci en az 3 karakter içermelidir");
    }
}