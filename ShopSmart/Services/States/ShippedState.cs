namespace ShopSmart.Services.States;

using ShopSmart.Enums;

public class ShippedState : IOrderState
{
    public OrderStatus  Status    => OrderStatus.Shipped;
    public OrderStatus? Next      => OrderStatus.Delivered;
    public bool         CanCancel => true;
    public string?      NextLabel => $"Advance to {Next}";
}
