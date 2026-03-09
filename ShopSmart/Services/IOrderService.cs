namespace ShopSmart.Services;

using ShopSmart.Models;
using ShopSmart.Services.Payments;
using ShopSmart.Services.States;

public interface IOrderService
{
    /// <summary>
    /// Processes checkout using the supplied payment strategy. The strategy validates
    /// and executes the payment before stock is reduced. Throws <see cref="ValidationException"/>
    /// on any failure.
    /// </summary>
    Order Checkout(User user, IPaymentStrategy paymentStrategy);

    /// <summary>Returns all orders for the user sorted by most recent first.</summary>
    IReadOnlyList<Order> GetOrderHistory(int userId);

    /// <summary>Returns the payment for a given order, or null if not found.</summary>
    Payment? GetPaymentForOrder(int orderId);

    /// <summary>Returns all orders across all users, sorted by most recent first.</summary>
    IReadOnlyList<Order> GetAllOrders();

    /// <summary>
    /// Returns the current state object for the given order.
    /// Throws <see cref="ValidationException"/> if the order is not found.
    /// </summary>
    IOrderState GetOrderState(int orderId);

    /// <summary>
    /// Advances the order to its next sequential status.
    /// Throws <see cref="ValidationException"/> if the order is not found or is in a terminal state.
    /// </summary>
    Order AdvanceOrderStatus(int orderId);

    /// <summary>
    /// Cancels the order. Throws <see cref="ValidationException"/> if already terminal.
    /// </summary>
    Order CancelOrder(int orderId);

    /// <summary>Returns aggregated sales metrics across all orders.</summary>
    SalesReport GetSalesReport();
}
