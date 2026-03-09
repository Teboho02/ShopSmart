namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class RestockProductView
{
    private readonly IProductService _productService;

    public RestockProductView(IProductService productService) =>
        _productService = productService;

    /// <summary>Shows active products, lets the admin pick one and add stock units. Loops until back.</summary>
    public void Run()
    {
        while (true)
        {
            ConsoleHelper.ClearScreen("Restock Product");
            var products = _productService.GetAllActive();

            if (products.Count == 0)
            {
                ConsoleHelper.WriteWarning("No active products.");
                ConsoleHelper.PressAnyKey();
                return;
            }

            RenderStockTable(products);
            Console.WriteLine();
            Console.Write("  Enter Product ID to restock (or 0 to go back): ");
            string idInput = Console.ReadLine()?.Trim() ?? "0";

            if (idInput == "0" || idInput == string.Empty)
                return;

            if (!int.TryParse(idInput, out int productId))
            {
                ConsoleHelper.WriteError("Please enter a valid product ID.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            var product = _productService.GetActiveById(productId);
            if (product is null)
            {
                ConsoleHelper.WriteError("Product not found.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            Console.WriteLine();
            ConsoleHelper.WriteInfo($"  Current stock: {product.Stock} unit(s)");
            Console.Write("  Units to add : ");
            string amountInput = Console.ReadLine()?.Trim() ?? "";

            if (!int.TryParse(amountInput, out int amount))
            {
                ConsoleHelper.WriteError("Please enter a valid quantity.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            try
            {
                var updated = _productService.RestockProduct(productId, amount);
                Console.WriteLine();
                ConsoleHelper.WriteSuccess(
                    $"Stock updated. {updated.Name} now has {updated.Stock} unit(s) in stock.");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine();
                ConsoleHelper.WriteError(ex.Message);
            }

            ConsoleHelper.PressAnyKey();
        }
    }

    private static void RenderStockTable(IReadOnlyList<Product> products)
    {
        Console.WriteLine();
        Console.WriteLine($"  {"ID",-5} {"Name",-30} {"Category",-14} {"Stock",5}");
        Console.WriteLine($"  {new string('─', 58)}");
        foreach (var p in products)
            Console.WriteLine($"  {p.Id,-5} {p.Name,-30} {p.Category,-14} {p.Stock,5}");
    }
}
