using MediatR;

namespace Catalog.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
}