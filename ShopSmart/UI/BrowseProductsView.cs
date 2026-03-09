namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class BrowseProductsView
{
    private readonly IProductService _productService;

    public BrowseProductsView(IProductService productService) =>
        _productService = productService;

    /// <summary>
    /// Shows the product catalogue, then prompts for a product ID to view details.
    /// Returns when the user enters 0 or leaves input empty.
    /// </summary>
    public void Run()
    {
        while (true)
        {
            ConsoleHelper.ClearScreen("Browse Products");
            ShowProductList();

            Console.WriteLine();
            Console.Write("  Enter a Product ID for details, or 0 to go back: ");
            string input = Console.ReadLine()?.Trim() ?? "0";

            if (input == "0" || input == string.Empty)
                return;

            if (!int.TryParse(input, out int id))
            {
                ConsoleHelper.WriteError("Please enter a valid product ID.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            var product = _productService.GetActiveById(id);
            if (product is null)
            {
                ConsoleHelper.WriteError($"No active product found with ID {id}.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            ShowProductDetail(product);
        }
    }

    // --- Private helpers ---

    private void ShowProductList()
    {
        var products = _productService.GetAllActive();

        if (products.Count == 0)
        {
            ConsoleHelper.WriteWarning("No products are currently available.");
            return;
        }

        var groups = products.GroupBy(p => p.Category);

        foreach (var group in groups)
        {
            Console.WriteLine();
            ConsoleHelper.WriteHeading(group.Key);

            foreach (var p in group)
            {
                string stockLabel = p.Stock > 0
                    ? $"Stock: {p.Stock,-4}"
                    : "[OUT OF STOCK]";

                ConsoleColor stockColour = p.Stock > 0
                    ? ConsoleColor.Gray
                    : ConsoleColor.DarkYellow;

                Console.Write($"  [{p.Id,3}] {p.Name,-30} {p.Price,8:C}   ");
                WriteColoured(stockLabel, stockColour);
            }
        }
    }

    private static void ShowProductDetail(Product product)
    {
        ConsoleHelper.ClearScreen("Product Details");
        Console.WriteLine();

        ConsoleHelper.WriteInfo($"  ID          : {product.Id}");
        ConsoleHelper.WriteInfo($"  Name        : {product.Name}");
        ConsoleHelper.WriteInfo($"  Category    : {product.Category}");
        ConsoleHelper.WriteInfo($"  Description : {product.Description}");
        ConsoleHelper.WriteInfo($"  Price       : {product.Price:C}");

        if (product.Stock > 0)
            ConsoleHelper.WriteSuccess($"  Stock       : {product.Stock} units available");
        else
            ConsoleHelper.WriteWarning("  Stock       : Out of stock");

        ConsoleHelper.WriteInfo("  Rating      : No ratings yet");

        Console.WriteLine();
        ConsoleHelper.WriteHeading("Reviews");
        ConsoleHelper.WriteWarning("  No reviews yet.");

        Console.WriteLine();
        ConsoleHelper.PressAnyKey();
    }

    private static void WriteColoured(string text, ConsoleColor colour)
    {
        ConsoleColor previous = Console.ForegroundColor;
        Console.ForegroundColor = colour;
        Console.WriteLine(text);
        Console.ForegroundColor = previous;
    }
}
