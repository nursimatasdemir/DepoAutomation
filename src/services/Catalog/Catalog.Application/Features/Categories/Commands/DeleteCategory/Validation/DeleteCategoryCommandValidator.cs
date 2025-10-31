using FluentValidation;

namespace Catalog.Application.Features.Categories.Commands.DeleteCategory.Validation;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Silinecek Kategori ID alanı boş bırakılamaz!");
    }   
}