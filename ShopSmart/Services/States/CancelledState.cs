namespace ShopSmart.Services.States;

using ShopSmart.Enums;

public class CancelledState : IOrderState
{
    public OrderStatus  Status    => OrderStatus.Cancelled;
    public OrderStatus? Next      => null;
    public bool         CanCancel => false;
    public string?      NextLabel => null;
}
