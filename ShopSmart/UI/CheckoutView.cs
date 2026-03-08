namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class CheckoutView
{
    private readonly ICartService  _cartService;
    private readonly IOrderService _orderService;

    public CheckoutView(ICartService cartService, IOrderService orderService)
    {
        _cartService  = cartService;
        _orderService = orderService;
    }

    /// <summary>
    /// Shows the order summary and wallet balance, prompts for confirmation,
    /// then calls OrderService.Checkout on approval.
    /// </summary>
    public void Run(User currentUser)
    {
        ConsoleHelper.ClearScreen("Checkout");
        var items = _cartService.GetCart(currentUser);

        if (items.Count == 0)
        {
            ConsoleHelper.WriteWarning("Your cart is empty. Nothing to checkout.");
            ConsoleHelper.PressAnyKey();
            return;
        }

        // Show order summary
        ConsoleHelper.WriteHeading("Order Summary");
        ViewCartView.RenderCartTable(items);

        decimal total = items.Sum(i => i.UnitPrice * i.Quantity);

        Console.WriteLine();
        ConsoleHelper.WriteInfo($"  Wallet balance : {currentUser.WalletBalance:C}");
        ConsoleHelper.WriteInfo($"  Order total    : {total:C}");

        decimal remaining = currentUser.WalletBalance - total;
        if (remaining < 0)
        {
            Console.WriteLine();
            ConsoleHelper.WriteError(
                $"Insufficient wallet balance. You need {Math.Abs(remaining):C} more.");
            ConsoleHelper.WriteWarning("Please top up your wallet before checking out.");
            ConsoleHelper.PressAnyKey();
            return;
        }

        ConsoleHelper.WriteInfo($"  Balance after  : {remaining:C}");

        // Confirmation prompt
        Console.WriteLine();
        Console.Write("  Confirm order? [Y/N]: ");
        string answer = Console.ReadLine()?.Trim().ToUpperInvariant() ?? "N";

        if (answer != "Y")
        {
            ConsoleHelper.WriteWarning("Checkout cancelled.");
            ConsoleHelper.PressAnyKey();
            return;
        }

        // Place order
        try
        {
            var order = _orderService.Checkout(currentUser);
            Console.WriteLine();
            ConsoleHelper.WriteSuccess($"Order #{order.Id} placed successfully!");
            ConsoleHelper.WriteSuccess($"New wallet balance: {currentUser.WalletBalance:C}");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine();
            ConsoleHelper.WriteError($"Checkout failed: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }
}
