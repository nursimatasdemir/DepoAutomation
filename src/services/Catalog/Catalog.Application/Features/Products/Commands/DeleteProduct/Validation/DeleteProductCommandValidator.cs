using FluentValidation;

namespace Catalog.Application.Features.Products.Commands.DeleteProduct.Validation;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Silinecek Kategori ID alanı boş bırakılamaz!");
    }
}