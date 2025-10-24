namespace Catalog.Application.DTOs;

public record CategoryDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public int ProductCount { get; set; }
}