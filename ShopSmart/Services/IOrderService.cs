namespace ShopSmart.Services;

using ShopSmart.Enums;
using ShopSmart.Models;

public interface IOrderService
{
    /// <summary>
    /// Processes checkout for the user: validates wallet balance and stock,
    /// reduces stock, creates Order and Payment records, deducts wallet, clears cart.
    /// Returns the placed Order. Throws <see cref="ValidationException"/> on any failure.
    /// </summary>
    Order Checkout(User user);

    /// <summary>Returns all orders for the user sorted by most recent first.</summary>
    IReadOnlyList<Order> GetOrderHistory(int userId);

    /// <summary>Returns the payment for a given order, or null if not found.</summary>
    Payment? GetPaymentForOrder(int orderId);

    /// <summary>Returns all orders across all users, sorted by most recent first.</summary>
    IReadOnlyList<Order> GetAllOrders();

    /// <summary>
    /// Updates an order's status. Throws <see cref="ValidationException"/> if the order
    /// is not found or is already in a terminal state (Delivered or Cancelled).
    /// </summary>
    Order UpdateOrderStatus(int orderId, OrderStatus newStatus);
}
