namespace Catalog.Domain;

public class Location
{
    public Guid Id { get; set; }
    public string? Code { get; set; } // @prop should be unique
    public string? Type { get; set; } // !!!bunu kontrol etmeyi unutma sonra 
}