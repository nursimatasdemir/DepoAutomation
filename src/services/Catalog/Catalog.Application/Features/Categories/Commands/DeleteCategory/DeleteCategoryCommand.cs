using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}