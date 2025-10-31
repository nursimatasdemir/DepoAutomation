using FluentValidation;

namespace Catalog.Application.Features.Locations.Commands.DeleteLocation.Validation;

public class DeleteLocationCommandValidator : AbstractValidator<DeleteLocationCommand>
{
    public DeleteLocationCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Silmek istediğiniz lokasyona ait ID blgisini giriniz.");
    }
}