using FluentValidation;
using Catalog.Application.Features.Products.Commands.UpdateProduct;

namespace Catalog.Application.Features.Products.Commands.UpdateProduct.Validation;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Güncellemek istediğiniz Ürün Id alanı boş bırakılamaz.");
        RuleFor(command => command.Name).NotEmpty().WithMessage("Ad alanı boş bırakılamaz");
        RuleFor(command => command.Sku).NotEmpty().WithMessage("SKU boş bırakılamaz")
            .MinimumLength(3).WithMessage("Sku (Stok Kodu) en az 3 karakterden olumalıdır.");
        RuleFor(command => command.CategoryId).NotEmpty()
            .WithMessage("Güncellemek istediğiniz ürünün ait olduğu Kategoriye ait ID bilgisini giriniz.");
    }
}