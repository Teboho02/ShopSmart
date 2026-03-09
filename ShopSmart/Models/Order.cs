namespace ShopSmart.Models;

using System.Text.Json.Serialization;
using ShopSmart.Enums;

public class Order
{
    public int             Id            { get; init; }
    public int             UserId        { get; init; }
    public DateTime        OrderDate     { get; init; }
    public OrderStatus     Status        { get; set; }
    public List<OrderItem> Items         { get; init; }
    public decimal         Total         { get; init; }
    public string          PaymentMethod { get; init; }

    [JsonConstructor]
    public Order(int id, int userId, DateTime orderDate, OrderStatus status,
                 List<OrderItem> items, decimal total, string paymentMethod)
    {
        Id            = id;
        UserId        = userId;
        OrderDate     = orderDate;
        Status        = status;
        Items         = items;
        Total         = total;
        PaymentMethod = paymentMethod;
    }
}
