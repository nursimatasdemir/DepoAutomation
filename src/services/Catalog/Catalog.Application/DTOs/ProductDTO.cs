namespace Catalog.Application.DTOs;

public record ProductDTO
{
    public Guid Id { get; init; } //PK
    public string? Sku { get; init; } = string.Empty;
    public string? Name { get; init; } =string.Empty;
    public string? Barcode { get; init; } = string.Empty;
    
    public string CategoryName { get; init; } = string.Empty;
    
    public bool IsActive { get; init; }

}