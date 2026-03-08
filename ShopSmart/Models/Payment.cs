namespace ShopSmart.Models;

using System.Text.Json.Serialization;
using ShopSmart.Enums;

public class Payment
{
    public int           Id      { get; init; }
    public int           OrderId { get; init; }
    public decimal       Amount  { get; init; }
    public string        Method  { get; init; }
    public PaymentStatus Status  { get; init; }
    public DateTime      PaidAt  { get; init; }

    [JsonConstructor]
    public Payment(int id, int orderId, decimal amount, string method, PaymentStatus status, DateTime paidAt)
    {
        Id      = id;
        OrderId = orderId;
        Amount  = amount;
        Method  = method;
        Status  = status;
        PaidAt  = paidAt;
    }
}
