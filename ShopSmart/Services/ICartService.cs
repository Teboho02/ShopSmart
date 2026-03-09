namespace ShopSmart.Services;

using ShopSmart.Models;

public interface ICartService
{
    /// <summary>
    /// Adds the specified quantity of a product to the user's cart.
    /// If the product is already in the cart, the quantity is incremented.
    /// Throws <see cref="ValidationException"/> if the product is invalid,
    /// inactive, or stock is insufficient.
    /// </summary>
    void AddToCart(User user, int productId, int quantity);

    /// <summary>Returns all cart items belonging to the user.</summary>
    IReadOnlyList<CartItem> GetCart(User user);

    /// <summary>
    /// Sets the quantity of a cart item to newQuantity.
    /// Passing 0 removes the item. Stock validation applies.
    /// Throws <see cref="ValidationException"/> on failure.
    /// </summary>
    void UpdateQuantity(User user, int productId, int newQuantity);
}
