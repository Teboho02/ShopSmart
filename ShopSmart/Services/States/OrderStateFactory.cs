namespace ShopSmart.Services.States;

using ShopSmart.Enums;

public static class OrderStateFactory
{
    public static IOrderState For(OrderStatus status) => status switch
    {
        OrderStatus.Pending    => new PendingState(),
        OrderStatus.Processing => new ProcessingState(),
        OrderStatus.Shipped    => new ShippedState(),
        OrderStatus.Delivered  => new DeliveredState(),
        OrderStatus.Cancelled  => new CancelledState(),
        _                      => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };
}
