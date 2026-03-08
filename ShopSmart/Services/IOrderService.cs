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
}
