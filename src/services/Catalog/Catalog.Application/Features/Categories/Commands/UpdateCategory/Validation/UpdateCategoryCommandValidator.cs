using Catalog.Application.Features.Categories.Commands.UpdateCategory;
using FluentValidation;
namespace Catalog.Application.Features.Categories.Commands.UpdateCategory.Validation;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty().WithMessage("Kategori Adı alanı boş bırakılamaz!")
            .MaximumLength(100).WithMessage("Kategori Adı en fazla 100 karakter içerebilir.");
    }
}