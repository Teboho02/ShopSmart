namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;
using ShopSmart.Services.Payments;

public class CheckoutView
{
    private readonly ICartService          _cartService;
    private readonly IOrderService         _orderService;
    private readonly PaymentStrategyFactory _factory;

    public CheckoutView(ICartService cartService, IOrderService orderService,
                        PaymentStrategyFactory factory)
    {
        _cartService  = cartService;
        _orderService = orderService;
        _factory      = factory;
    }

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
        ConsoleHelper.WriteInfo($"  Order total : {total:C}");

        // Select payment method
        Console.WriteLine();
        int methodChoice = MenuRenderer.Show("Select Payment Method",
            ["Wallet", "EFT", "PayPal", "Voucher"]);

        IPaymentStrategy strategy;

        switch (methodChoice)
        {
            case 1: // Wallet
                Console.WriteLine();
                ConsoleHelper.WriteInfo($"  Wallet balance : {currentUser.WalletBalance:C}");
                decimal remaining = currentUser.WalletBalance - total;
                if (remaining < 0)
                {
                    Console.WriteLine();
                    ConsoleHelper.WriteError(
                        $"Insufficient wallet balance. You need {Math.Abs(remaining):C} more.");
                    ConsoleHelper.WriteWarning("Please top up your wallet or choose another payment method.");
                    ConsoleHelper.PressAnyKey();
                    return;
                }
                ConsoleHelper.WriteInfo($"  Balance after  : {remaining:C}");
                strategy = _factory.Wallet();
                break;

            case 2: // EFT
                Console.WriteLine();
                ConsoleHelper.WriteInfo("  Transfer the exact amount to:");
                Console.WriteLine("  Bank    : FNB");
                Console.WriteLine("  Account : 62312345678");
                Console.WriteLine("  Branch  : 250655");
                Console.WriteLine($"  Amount  : {total:C}");
                Console.WriteLine("  Ref     : Your order number (shown after placing)");
                Console.WriteLine();
                ConsoleHelper.WriteWarning("  Your order will be marked Pending until payment is confirmed.");
                strategy = _factory.Eft();
                break;

            case 3: // PayPal
                Console.WriteLine();
                Console.Write("  PayPal email: ");
                string email = Console.ReadLine()?.Trim() ?? string.Empty;
                if (!email.Contains('@'))
                {
                    ConsoleHelper.WriteError("Please enter a valid PayPal email address.");
                    ConsoleHelper.PressAnyKey();
                    return;
                }
                strategy = _factory.PayPal(email);
                break;

            case 4: // Voucher
                Console.WriteLine();
                Console.Write("  Voucher code: ");
                string code = Console.ReadLine()?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(code))
                {
                    ConsoleHelper.WriteError("Please enter a voucher code.");
                    ConsoleHelper.PressAnyKey();
                    return;
                }
                ConsoleHelper.WriteInfo("  Any shortfall will be charged to your wallet.");
                strategy = _factory.Voucher(code);
                break;

            default:
                return;
        }

        // Confirm
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
            var order = _orderService.Checkout(currentUser, strategy);
            Console.WriteLine();

            if (strategy.MethodName == "EFT")
            {
                ConsoleHelper.WriteSuccess($"Order #{order.Id} placed.");
                ConsoleHelper.WriteWarning($"Awaiting EFT payment of {total:C}. Use Order #{order.Id} as your reference.");
            }
            else
            {
                ConsoleHelper.WriteSuccess($"Order #{order.Id} placed successfully!");
                if (strategy.MethodName == "Wallet")
                    ConsoleHelper.WriteSuccess($"New wallet balance: {currentUser.WalletBalance:C}");
            }
        }
        catch (ValidationException ex)
        {
            Console.WriteLine();
            ConsoleHelper.WriteError($"Checkout failed: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }
}
