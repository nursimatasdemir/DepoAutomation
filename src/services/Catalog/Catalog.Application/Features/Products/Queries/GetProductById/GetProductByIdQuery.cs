using Catalog.Application.DTOs;
using MediatR;
namespace Catalog.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<ProductDTO?>
{
    public Guid Id { get; set; }
}