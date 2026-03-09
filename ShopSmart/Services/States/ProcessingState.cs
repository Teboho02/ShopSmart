namespace ShopSmart.Services.States;

using ShopSmart.Enums;

public class ProcessingState : IOrderState
{
    public OrderStatus  Status    => OrderStatus.Processing;
    public OrderStatus? Next      => OrderStatus.Shipped;
    public bool         CanCancel => true;
    public string?      NextLabel => $"Advance to {Next}";
}
