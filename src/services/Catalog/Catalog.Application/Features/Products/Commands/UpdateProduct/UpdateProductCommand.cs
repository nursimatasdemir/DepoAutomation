using Catalog.Application.DTOs;
using Catalog.Domain;
using  MediatR;

namespace Catalog.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest<ProductDTO?>
{
    public Guid Id { get; set; }

    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }


}