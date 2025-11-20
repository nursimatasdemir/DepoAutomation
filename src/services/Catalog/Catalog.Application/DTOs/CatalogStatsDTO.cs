namespace Catalog.Application.DTOs;

public class CatalogStatsDTO
{
    public int ProductCount { get; set; }
    public int CategoryCount { get; set; }
    public int LocationCount { get; set; }
    public int ActiveProductCount { get; set; }
}