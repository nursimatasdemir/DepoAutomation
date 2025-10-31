using FluentValidation;
using Catalog.Application.Features.Categories.Commands.CreateCategory;
namespace Catalog.Application.Features.Categories.Commands.CreateCategory.Validation;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty().WithMessage("Kategori Adı alanı boş bırakılamaz!")
            .MaximumLength(100).WithMessage("Kategori adı en fazla 100 karakter içerebilir.");
    }
}