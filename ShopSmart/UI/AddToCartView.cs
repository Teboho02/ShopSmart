namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class AddToCartView
{
    private readonly IProductService _productService;
    private readonly ICartService    _cartService;

    public AddToCartView(IProductService productService, ICartService cartService)
    {
        _productService = productService;
        _cartService    = cartService;
    }

    /// <summary>
    /// Shows available products, then prompts for a product ID and quantity.
    /// Loops until the user enters 0 to go back.
    /// </summary>
    public void Run(User currentUser)
    {
        while (true)
        {
            ConsoleHelper.ClearScreen("Add Product to Cart");
            ShowProductReference();

            Console.WriteLine();
            Console.Write("  Enter Product ID to add (or 0 to go back): ");
            string idInput = Console.ReadLine()?.Trim() ?? "0";

            if (idInput == "0" || idInput == string.Empty)
                return;

            if (!int.TryParse(idInput, out int productId))
            {
                ConsoleHelper.WriteError("Please enter a valid product ID.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            Console.Write("  Quantity: ");
            string qtyInput = Console.ReadLine()?.Trim() ?? "0";

            if (!int.TryParse(qtyInput, out int quantity))
            {
                ConsoleHelper.WriteError("Please enter a valid quantity.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            try
            {
                _cartService.AddToCart(currentUser, productId, quantity);

                // Retrieve the item to show the snapshotted name and price
                var item = _cartService.GetCart(currentUser)
                                       .First(c => c.ProductId == productId);

                Console.WriteLine();
                ConsoleHelper.WriteSuccess(
                    $"Added to cart: {item.ProductName} x{quantity} @ {item.UnitPrice:C}");
                ConsoleHelper.PressAnyKey();
            }
            catch (ValidationException ex)
            {
                Console.WriteLine();
                ConsoleHelper.WriteError(ex.Message);
                ConsoleHelper.PressAnyKey();
            }
        }
    }

    // --- Private helpers ---

    private void ShowProductReference()
    {
        var products = _productService.GetAllActive();
        if (products.Count == 0)
        {
            ConsoleHelper.WriteWarning("No products are currently available.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("  Available products:");
        Console.WriteLine();

        foreach (var p in products)
        {
            string stock = p.Stock > 0 ? $"Stock: {p.Stock}" : "[OUT OF STOCK]";
            ConsoleColor colour = p.Stock > 0 ? ConsoleColor.Gray : ConsoleColor.DarkYellow;

            Console.Write($"  [{p.Id,3}] {p.Name,-30} {p.Price,8:C}   ");
            WriteColoured(stock, colour);
        }
    }

    private static void WriteColoured(string text, ConsoleColor colour)
    {
        ConsoleColor previous = Console.ForegroundColor;
        Console.ForegroundColor = colour;
        Console.WriteLine(text);
        Console.ForegroundColor = previous;
    }
}
