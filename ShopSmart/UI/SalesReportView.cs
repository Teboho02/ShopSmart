namespace ShopSmart.UI;

using ShopSmart.Services;

public class SalesReportView(IOrderService orderService)
{
    public void Run()
    {
        ConsoleHelper.ClearScreen("Sales Report");
        var report = orderService.GetSalesReport();

        // ── Revenue Summary ──────────────────────────────────────────────────
        Console.WriteLine();
        ConsoleHelper.WriteHeading("Revenue Summary");
        Console.WriteLine();

        if (report.TotalOrders == 0)
        {
            ConsoleHelper.WriteWarning("  No sales data available.");
            Console.WriteLine();
            ConsoleHelper.PressAnyKey();
            return;
        }

        Console.WriteLine($"  {"Total Orders",-22}: {report.TotalOrders,6}");
        ConsoleHelper.WriteSuccess($"  {"Total Revenue",-22}: {report.TotalRevenue,9:C}");
        Console.WriteLine($"  {"Avg Order Value",-22}: {report.AverageOrderValue,9:C}");

        // ── Orders by Status ─────────────────────────────────────────────────
        Console.WriteLine();
        ConsoleHelper.WriteHeading("Orders by Status");
        Console.WriteLine();

        foreach (var (status, count) in report.OrdersByStatus)
        {
            if (count == 0) continue;
            Console.WriteLine($"  {status,-22}: {count,6}");
        }

        // ── Top 5 Products ───────────────────────────────────────────────────
        if (report.TopProducts.Count > 0)
        {
            Console.WriteLine();
            ConsoleHelper.WriteHeading("Top Selling Products");
            Console.WriteLine();

            Console.WriteLine($"  {"#",-4} {"Product",-30} {"Units Sold",10}  {"Revenue",10}");
            Console.WriteLine($"  {new string('─', 58)}");

            int rank = 1;
            foreach (var (name, units, revenue) in report.TopProducts)
            {
                Console.WriteLine($"  {rank,-4} {name,-30} {units,10}  {revenue,10:C}");
                rank++;
            }

            Console.WriteLine($"  {new string('─', 58)}");
        }

        Console.WriteLine();
        ConsoleHelper.PressAnyKey();
    }
}
