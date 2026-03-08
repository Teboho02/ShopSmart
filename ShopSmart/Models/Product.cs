namespace ShopSmart.Models;

using System.Text.Json.Serialization;

public class Product
{
    public int      Id          { get; init; }
    public string   Name        { get; set; }
    public string   Description { get; set; }
    public string   Category    { get; set; }
    public decimal  Price       { get; set; }
    public int      Stock       { get; set; }
    public bool     IsActive    { get; set; }
    public DateTime CreatedAt   { get; init; }

    /// <summary>Full constructor used by System.Text.Json to restore persisted products.</summary>
    [JsonConstructor]
    public Product(int id, string name, string description, string category,
                   decimal price, int stock, bool isActive, DateTime createdAt)
    {
        Id          = id;
        Name        = name;
        Description = description;
        Category    = category;
        Price       = price;
        Stock       = stock;
        IsActive    = isActive;
        CreatedAt   = createdAt;
    }

    /// <summary>Convenience constructor for creating new products; defaults to active, CreatedAt now.</summary>
    public Product(int id, string name, string description, string category, decimal price, int stock)
        : this(id, name, description, category, price, stock, isActive: true, createdAt: DateTime.UtcNow) { }
}
