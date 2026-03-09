namespace ShopSmart.Data;

using ShopSmart.Models;

/// <summary>
/// In-memory cart store. Cart data is session-scoped and not persisted to disk.
/// </summary>
public class CartRepository
{
    private readonly AppData _data;

    public CartRepository(AppData data) => _data = data;

    public IReadOnlyList<CartItem> GetByUser(int userId) =>
        _data.CartItems.Where(c => c.UserId == userId).ToList().AsReadOnly();

    public CartItem? FindItem(int userId, int productId) =>
        _data.CartItems.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

    public void Add(CartItem item) =>
        _data.CartItems.Add(item);

    public void Remove(CartItem item) =>
        _data.CartItems.Remove(item);

    public void Clear(int userId) =>
        _data.CartItems.RemoveAll(c => c.UserId == userId);
}
