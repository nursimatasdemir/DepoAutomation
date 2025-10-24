using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest<CategoryDTO?>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}