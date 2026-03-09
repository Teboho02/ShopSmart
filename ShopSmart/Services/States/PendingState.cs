namespace ShopSmart.Services.States;

using ShopSmart.Enums;

public class PendingState : IOrderState
{
    public OrderStatus  Status    => OrderStatus.Pending;
    public OrderStatus? Next      => OrderStatus.Processing;
    public bool         CanCancel => true;
    public string?      NextLabel => $"Advance to {Next}";
}
