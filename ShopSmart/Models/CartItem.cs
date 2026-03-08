namespace ShopSmart.Models;

public class CartItem
{
    public int     UserId      { get; init; }
    public int     ProductId   { get; init; }
    public string  ProductName { get; init; }   // snapshotted at time of adding
    public decimal UnitPrice   { get; init; }   // snapshotted at time of adding
    public int     Quantity    { get; set; }

    public CartItem(int userId, int productId, string productName, decimal unitPrice, int quantity)
    {
        UserId      = userId;
        ProductId   = productId;
        ProductName = productName;
        UnitPrice   = unitPrice;
        Quantity    = quantity;
    }
}
