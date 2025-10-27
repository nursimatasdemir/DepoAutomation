namespace Catalog.Application.DTOs;

public class LocationDTO
{
    public Guid Id { get; set; }
    public string? Code { get; set; } = string.Empty;
    public string? Type { get; set; } = string.Empty;
}