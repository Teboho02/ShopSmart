namespace ShopSmart.UI;

using ShopSmart.Enums;
using ShopSmart.Models;
using ShopSmart.Services;

public class TrackOrderView
{
    private readonly IOrderService _orderService;

    public TrackOrderView(IOrderService orderService) =>
        _orderService = orderService;

    /// <summary>
    /// Prompts for an order number and displays its status with a visual pipeline. Loops until back.
    /// </summary>
    public void Run(User currentUser)
    {
        while (true)
        {
            ConsoleHelper.ClearScreen("Track Order");
            Console.WriteLine();
            Console.Write("  Enter Order # to track (or 0 to go back): ");
            string input = Console.ReadLine()?.Trim() ?? "0";

            if (input == "0" || input == string.Empty)
                return;

            if (!int.TryParse(input, out int orderId))
            {
                ConsoleHelper.WriteError("Please enter a valid order number.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            var order = _orderService.GetOrderHistory(currentUser.Id)
                                     .FirstOrDefault(o => o.Id == orderId);

            if (order is null)
            {
                ConsoleHelper.WriteError("Order not found or does not belong to you.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            Console.WriteLine();
            ConsoleHelper.WriteInfo($"  Order #{order.Id}");
            ConsoleHelper.WriteInfo($"  Date  : {order.OrderDate.ToLocalTime():yyyy-MM-dd HH:mm}");
            ConsoleHelper.WriteInfo($"  Total : {order.Total:C}");
            Console.WriteLine();

            RenderPipeline(order.Status);

            ConsoleHelper.PressAnyKey();
        }
    }

    private static void RenderPipeline(OrderStatus status)
    {
        if (status == OrderStatus.Cancelled)
        {
            ConsoleHelper.WriteError("  ✗  This order has been cancelled.");
            return;
        }

        // Steps in order; Delivered = index 3
        string[] steps = ["Pending", "Processing", "Shipped", "Delivered"];
        int currentIndex = status switch
        {
            OrderStatus.Pending    => 0,
            OrderStatus.Processing => 1,
            OrderStatus.Shipped    => 2,
            OrderStatus.Delivered  => 3,
            _                      => 0
        };

        Console.Write("  ");
        for (int i = 0; i < steps.Length; i++)
        {
            string marker = i <= currentIndex ? "●" : "○";
            Console.Write($"{marker} {steps[i]}");
            if (i < steps.Length - 1)
                Console.Write(" ──── ");
        }
        Console.WriteLine();
    }
}
