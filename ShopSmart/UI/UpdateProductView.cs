namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class UpdateProductView
{
    private readonly IProductService _productService;

    public UpdateProductView(IProductService productService) =>
        _productService = productService;

    /// <summary>Shows the product list, lets the admin pick one and edit its fields. Loops until back.</summary>
    public void Run()
    {
        while (true)
        {
            ConsoleHelper.ClearScreen("Update Product");
            var products = _productService.GetAllActive();

            if (products.Count == 0)
            {
                ConsoleHelper.WriteWarning("No products available.");
                ConsoleHelper.PressAnyKey();
                return;
            }

            RenderProductTable(products);
            Console.WriteLine();
            Console.Write("  Enter Product ID to update (or 0 to go back): ");
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

            // Collect updated fields — empty input keeps the current value
            Console.WriteLine();
            ConsoleHelper.WriteInfo($"  Editing: {product.Name} (ID #{product.Id})");
            ConsoleHelper.WriteInfo("  Press Enter on any field to keep the current value.");
            Console.WriteLine();

            Console.Write($"  Name        [{product.Name}]: ");
            string nameInput = Console.ReadLine()?.Trim() ?? "";
            string name = nameInput == string.Empty ? product.Name : nameInput;

            Console.Write($"  Description [{product.Description}]: ");
            string descInput = Console.ReadLine()?.Trim() ?? "";
            string description = descInput == string.Empty ? product.Description : descInput;

            Console.Write($"  Category    [{product.Category}]: ");
            string catInput = Console.ReadLine()?.Trim() ?? "";
            string category = catInput == string.Empty ? product.Category : catInput;

            Console.Write($"  Price       [{product.Price:C}]: ");
            string priceInput = Console.ReadLine()?.Trim() ?? "";
            decimal price;
            if (priceInput == string.Empty)
                price = product.Price;
            else if (!decimal.TryParse(priceInput, out price))
            {
                ConsoleHelper.WriteError("Invalid price — update cancelled.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            Console.Write($"  Stock       [{product.Stock}]: ");
            string stockInput = Console.ReadLine()?.Trim() ?? "";
            int stock;
            if (stockInput == string.Empty)
                stock = product.Stock;
            else if (!int.TryParse(stockInput, out stock))
            {
                ConsoleHelper.WriteError("Invalid stock quantity — update cancelled.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            try
            {
                _productService.UpdateProduct(productId, name, description, category, price, stock);
                Console.WriteLine();
                ConsoleHelper.WriteSuccess("Product updated successfully.");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine();
                ConsoleHelper.WriteError(ex.Message);
            }

            ConsoleHelper.PressAnyKey();
        }
    }

    private static void RenderProductTable(IReadOnlyList<Product> products)
    {
        Console.WriteLine();
        Console.WriteLine($"  {"ID",-5} {"Name",-30} {"Category",-14} {"Price",8}  {"Stock",5}");
        Console.WriteLine($"  {new string('─', 66)}");
        foreach (var p in products)
            Console.WriteLine($"  {p.Id,-5} {p.Name,-30} {p.Category,-14} {p.Price,8:C}  {p.Stock,5}");
    }
}
