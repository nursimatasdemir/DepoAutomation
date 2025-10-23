namespace Catalog.Domain;

public class Product
{
    public Guid Id { get; set; } //PK
    public string? Sku { get; set; } // @prop SKU short for Stock Keeping Unit
    public string? Name { get; set; }
    public string? Barcode { get; set; }
    
    public Guid CategoryId { get; set; } //FK 
    public Category? Category { get; set; }
}