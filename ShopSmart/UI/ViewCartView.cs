namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class ViewCartView
{
    private readonly ICartService _cartService;

    public ViewCartView(ICartService cartService) =>
        _cartService = cartService;

    /// <summary>Displays the user's cart as a formatted table then waits for a keypress.</summary>
    public void Run(User currentUser)
    {
        ConsoleHelper.ClearScreen("Your Cart");
        var items = _cartService.GetCart(currentUser);

        if (items.Count == 0)
        {
            Console.WriteLine();
            ConsoleHelper.WriteWarning("Your cart is empty.");
            ConsoleHelper.PressAnyKey();
            return;
        }

        RenderCartTable(items);
        ConsoleHelper.PressAnyKey();
    }

    /// <summary>Renders the cart table. Can be reused by other views (e.g. Checkout).</summary>
    public static void RenderCartTable(IReadOnlyList<CartItem> items)
    {
        Console.WriteLine();
        Console.WriteLine($"  {"Product",-30} {"Qty",4}  {"Unit Price",10}  {"Subtotal",10}");
        Console.WriteLine($"  {new string('─', 60)}");

        foreach (var item in items)
        {
            decimal subtotal = item.UnitPrice * item.Quantity;
            Console.WriteLine($"  {item.ProductName,-30} {item.Quantity,4}  {item.UnitPrice,10:C}  {subtotal,10:C}");
        }

        Console.WriteLine($"  {new string('─', 60)}");

        decimal total = items.Sum(i => i.UnitPrice * i.Quantity);
        ConsoleHelper.WriteSuccess($"  {"TOTAL",-30}       {total,22:C}");
    }
}
