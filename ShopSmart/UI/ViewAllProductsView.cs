namespace ShopSmart.UI;

using ShopSmart.Services;

public class ViewAllProductsView
{
    private readonly IProductService _productService;

    public ViewAllProductsView(IProductService productService) =>
        _productService = productService;

    /// <summary>Displays every product (active and inactive) with a status indicator, then waits for a keypress.</summary>
    public void Run()
    {
        ConsoleHelper.ClearScreen("All Products");
        var all = _productService.GetAll();

        if (all.Count == 0)
        {
            Console.WriteLine();
            ConsoleHelper.WriteWarning("No products found.");
            ConsoleHelper.PressAnyKey();
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"  {"ID",-5} {"Name",-30} {"Category",-14} {"Price",8}  {"Stock",5}  Status");
        Console.WriteLine($"  {new string('─', 74)}");

        foreach (var p in all)
        {
            Console.Write($"  {p.Id,-5} {p.Name,-30} {p.Category,-14} {p.Price,8:C}  {p.Stock,5}  ");

            if (p.IsActive)
                ConsoleHelper.WriteSuccess("Active");
            else
                ConsoleHelper.WriteWarning("Inactive");
        }

        Console.WriteLine($"  {new string('─', 74)}");

        int activeCount   = all.Count(p => p.IsActive);
        int inactiveCount = all.Count - activeCount;
        Console.WriteLine();
        ConsoleHelper.WriteInfo($"  Total: {all.Count} product(s)  ({activeCount} active, {inactiveCount} inactive)");

        ConsoleHelper.PressAnyKey();
    }
}
