namespace ShopSmart.Services.States;

using ShopSmart.Enums;

public class DeliveredState : IOrderState
{
    public OrderStatus  Status    => OrderStatus.Delivered;
    public OrderStatus? Next      => null;
    public bool         CanCancel => false;
    public string?      NextLabel => null;
}
