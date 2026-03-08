namespace ShopSmart.Data;

using ShopSmart.Models;

/// <summary>
/// Central in-memory data store. One instance is created at startup and
/// injected into all repositories. Acts as the application's "database".
/// ID generation is handled by each repository after loading persisted data.
/// </summary>
public class AppData
{
    public List<User>     Users     { get; } = [];
    public List<Product>  Products  { get; } = [];
    public List<CartItem> CartItems { get; } = [];   // session-only, not persisted
    public List<Order>    Orders    { get; } = [];
    public List<Payment>  Payments  { get; } = [];
}
