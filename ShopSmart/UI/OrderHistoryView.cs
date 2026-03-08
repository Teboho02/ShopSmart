namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class OrderHistoryView
{
    private readonly IOrderService _orderService;

    public OrderHistoryView(IOrderService orderService) =>
        _orderService = orderService;

    /// <summary>Shows the user's order list; allows drill-down to itemised detail. Loops until back.</summary>
    public void Run(User currentUser)
    {
        while (true)
        {
            ConsoleHelper.ClearScreen("Order History");
            var orders = _orderService.GetOrderHistory(currentUser.Id);

            if (orders.Count == 0)
            {
                Console.WriteLine();
                ConsoleHelper.WriteWarning("You have no orders yet.");
                ConsoleHelper.PressAnyKey();
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"  {"#",-6} {"Date",-20} {"Status",-14} {"Total",10}");
            Console.WriteLine($"  {new string('─', 54)}");

            foreach (var o in orders)
                Console.WriteLine($"  {o.Id,-6} {o.OrderDate.ToLocalTime():yyyy-MM-dd HH:mm,-20} {o.Status,-14} {o.Total,10:C}");

            Console.WriteLine($"  {new string('─', 54)}");
            Console.WriteLine();
            Console.Write("  Enter Order # for details (or 0 to go back): ");
            string input = Console.ReadLine()?.Trim() ?? "0";

            if (input == "0" || input == string.Empty)
                return;

            if (!int.TryParse(input, out int orderId))
            {
                ConsoleHelper.WriteError("Please enter a valid order number.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            var order = orders.FirstOrDefault(o => o.Id == orderId);
            if (order is null)
            {
                ConsoleHelper.WriteError("Order not found.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            ShowOrderDetail(order);
        }
    }

    private void ShowOrderDetail(Order order)
    {
        ConsoleHelper.ClearScreen($"Order #{order.Id} Details");
        Console.WriteLine();
        ConsoleHelper.WriteInfo($"  Date    : {order.OrderDate.ToLocalTime():yyyy-MM-dd HH:mm}");
        ConsoleHelper.WriteInfo($"  Status  : {order.Status}");
        ConsoleHelper.WriteInfo($"  Payment : {order.PaymentMethod}");

        Console.WriteLine();
        Console.WriteLine($"  {"Product",-30} {"Qty",4}  {"Unit Price",10}  {"Subtotal",10}");
        Console.WriteLine($"  {new string('─', 60)}");

        foreach (var item in order.Items)
        {
            decimal subtotal = item.UnitPrice * item.Quantity;
            Console.WriteLine($"  {item.ProductName,-30} {item.Quantity,4}  {item.UnitPrice,10:C}  {subtotal,10:C}");
        }

        Console.WriteLine($"  {new string('─', 60)}");
        ConsoleHelper.WriteSuccess($"  {"TOTAL",-30}       {order.Total,22:C}");

        var payment = _orderService.GetPaymentForOrder(order.Id);
        if (payment is not null)
        {
            Console.WriteLine();
            ConsoleHelper.WriteInfo(
                $"  Payment #{payment.Id}: {payment.Amount:C} via {payment.Method}" +
                $" — {payment.Status} at {payment.PaidAt.ToLocalTime():yyyy-MM-dd HH:mm}");
        }

        ConsoleHelper.PressAnyKey();
    }
}
