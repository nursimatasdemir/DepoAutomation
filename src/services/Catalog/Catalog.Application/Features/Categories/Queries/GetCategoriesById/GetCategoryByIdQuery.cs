using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Categories.Queries.GetCategoriesById;

public class GetCategoryByIdQuery : IRequest<CategoryDTO?>
{
    public Guid Id { get; set; }
}