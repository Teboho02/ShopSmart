namespace ShopSmart.Models;

using System.Text.Json.Serialization;

public class OrderItem
{
    public int     ProductId   { get; init; }
    public string  ProductName { get; init; }
    public decimal UnitPrice   { get; init; }
    public int     Quantity    { get; init; }

    public decimal LineTotal => UnitPrice * Quantity;

    [JsonConstructor]
    public OrderItem(int productId, string productName, decimal unitPrice, int quantity)
    {
        ProductId   = productId;
        ProductName = productName;
        UnitPrice   = unitPrice;
        Quantity    = quantity;
    }
}
