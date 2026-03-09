namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class DeleteProductView
{
    private readonly IProductService _productService;

    public DeleteProductView(IProductService productService) =>
        _productService = productService;

    /// <summary>Shows the product list, lets the admin pick one and soft-delete it after confirmation. Loops until back.</summary>
    public void Run()
    {
        while (true)
        {
            ConsoleHelper.ClearScreen("Delete Product");
            var products = _productService.GetAllActive();

            if (products.Count == 0)
            {
                ConsoleHelper.WriteWarning("No products to delete.");
                ConsoleHelper.PressAnyKey();
                return;
            }

            RenderProductTable(products);
            Console.WriteLine();
            Console.Write("  Enter Product ID to delete (or 0 to go back): ");
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
            ConsoleHelper.WriteWarning($"  You are about to delete: {product.Name}");
            Console.Write("  Confirm deletion? [Y/N]: ");
            string answer = Console.ReadLine()?.Trim().ToUpperInvariant() ?? "N";

            if (answer != "Y")
            {
                ConsoleHelper.WriteWarning("Deletion cancelled.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            try
            {
                string deletedName = product.Name;
                _productService.DeleteProduct(productId);
                Console.WriteLine();
                ConsoleHelper.WriteSuccess($"Product '{deletedName}' has been deleted.");
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
