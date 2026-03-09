namespace ShopSmart.UI;

using ShopSmart.Services;

public class AddProductView
{
    private readonly IProductService _productService;

    public AddProductView(IProductService productService) =>
        _productService = productService;

    /// <summary>Collects product details from the admin and creates the new product.</summary>
    public void Run()
    {
        ConsoleHelper.ClearScreen("Add Product");
        Console.WriteLine();

        string name        = ConsoleHelper.Prompt("  Product name  : ");
        string description = ConsoleHelper.Prompt("  Description   : ");
        string category    = ConsoleHelper.Prompt("  Category      : ");

        Console.Write("  Price ($)     : ");
        string priceInput = Console.ReadLine()?.Trim() ?? "";
        if (!decimal.TryParse(priceInput, out decimal price))
        {
            ConsoleHelper.WriteError("Please enter a valid price.");
            ConsoleHelper.PressAnyKey();
            return;
        }

        Console.Write("  Initial stock : ");
        string stockInput = Console.ReadLine()?.Trim() ?? "";
        if (!int.TryParse(stockInput, out int stock))
        {
            ConsoleHelper.WriteError("Please enter a valid stock quantity.");
            ConsoleHelper.PressAnyKey();
            return;
        }

        try
        {
            var product = _productService.AddProduct(name, description, category, price, stock);
            Console.WriteLine();
            ConsoleHelper.WriteSuccess($"Product '{product.Name}' (ID #{product.Id}) added successfully.");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine();
            ConsoleHelper.WriteError(ex.Message);
        }

        ConsoleHelper.PressAnyKey();
    }
}
