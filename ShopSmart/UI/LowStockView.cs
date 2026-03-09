namespace ShopSmart.UI;

using ShopSmart.Services;

public class LowStockView(IProductService productService)
{
    private const int Threshold = 5;

    public void Run()
    {
        ConsoleHelper.ClearScreen("Low Stock Report");
        var products = productService.GetLowStock(Threshold);

        if (products.Count == 0)
        {
            ConsoleHelper.WriteSuccess("All products are sufficiently stocked.");
        }
        else
        {
            Console.WriteLine($"  {"Name",-30} {"Category",-18} {"Stock",6}");
            Console.WriteLine(new string('─', 58));

            foreach (var p in products)
            {
                Console.Write($"  {p.Name,-30} {p.Category,-18} ");
                if (p.Stock == 0)
                    ConsoleHelper.WriteErrorInline($"{p.Stock,6}");
                else
                    ConsoleHelper.WriteWarningInline($"{p.Stock,6}");
                Console.WriteLine();
            }

            Console.WriteLine(new string('─', 58));
            Console.WriteLine();
            ConsoleHelper.WriteWarning($"{products.Count} product(s) at or below {Threshold} units.");
        }

        ConsoleHelper.PressAnyKey();
    }
}
