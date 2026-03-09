namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class SearchProductsView
{
    private readonly IProductService _productService;

    public SearchProductsView(IProductService productService) =>
        _productService = productService;

    /// <summary>
    /// Prompts for a search term, displays matching products, and allows
    /// drilling into a product detail. Returns when the user leaves the term empty.
    /// </summary>
    public void Run()
    {
        while (true)
        {
            ConsoleHelper.ClearScreen("Search Products");
            Console.WriteLine();

            string term = ConsoleHelper.Prompt("Search term (or leave empty to go back)");

            if (term == string.Empty)
                return;

            var results = _productService.SearchActive(term);

            if (results.Count == 0)
            {
                Console.WriteLine();
                ConsoleHelper.WriteWarning($"No products found for '{term}'.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            ShowResults(results);
            PromptForDetail(results);
        }
    }

    // --- Private helpers ---

    private static void ShowResults(IReadOnlyList<Product> results)
    {
        Console.WriteLine();
        ConsoleHelper.WriteInfo($"  {results.Count} result(s) found:");
        Console.WriteLine();

        foreach (var p in results)
        {
            string stockLabel = p.Stock > 0
                ? $"Stock: {p.Stock}"
                : "[OUT OF STOCK]";

            ConsoleColor stockColour = p.Stock > 0 ? ConsoleColor.Gray : ConsoleColor.DarkYellow;

            Console.Write($"  [{p.Id,3}] {p.Name,-30} {p.Category,-14} {p.Price,8:C}   ");
            WriteColoured(stockLabel, stockColour);
        }
    }

    private void PromptForDetail(IReadOnlyList<Product> results)
    {
        while (true)
        {
            Console.WriteLine();
            Console.Write("  Enter a Product ID for details, or 0 to search again: ");
            string input = Console.ReadLine()?.Trim() ?? "0";

            if (input == "0" || input == string.Empty)
                return;

            if (!int.TryParse(input, out int id))
            {
                ConsoleHelper.WriteError("Please enter a valid product ID.");
                continue;
            }

            var product = _productService.GetActiveById(id);

            if (product is null || results.All(r => r.Id != id))
            {
                ConsoleHelper.WriteError($"Product ID {id} is not in the current results.");
                continue;
            }

            ShowProductDetail(product);
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
