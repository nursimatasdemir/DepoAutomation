using FluentValidation;

namespace Catalog.Application.Features.Products.Commands.CreateProduct.Validation;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty().WithMessage("Ürün adı boş olamaz!");
        RuleFor(command => command.Sku).NotEmpty().WithMessage("SKU (Stok Kodu) boş olamaz!")
            .MinimumLength(3).WithMessage("SKU (Stok Kodu) en az 3 karakterden oluşmalıdır!");
        RuleFor(command => command.CategoryId).NotEmpty().WithMessage("Kategori ID boş bırakılamaz!");
    }
}