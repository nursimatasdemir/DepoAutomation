using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Products.Queries.GetProducts;

public class GetProductsQuery : IRequest<List<ProductDTO>>
{
    
}