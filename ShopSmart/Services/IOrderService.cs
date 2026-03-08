namespace ShopSmart.Services;

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
}
