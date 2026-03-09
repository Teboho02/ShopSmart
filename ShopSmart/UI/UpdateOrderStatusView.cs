namespace ShopSmart.UI;

using ShopSmart.Enums;
using ShopSmart.Models;
using ShopSmart.Services;

public class UpdateOrderStatusView
{
    private readonly IOrderService   _orderService;
    private readonly IUserService    _userService;

    public UpdateOrderStatusView(IOrderService orderService, IUserService userService) =>
        (_orderService, _userService) = (orderService, userService);

    /// <summary>Lists all orders and lets the admin update the status of any non-terminal order.</summary>
    public void Run()
    {
        while (true)
        {
            ConsoleHelper.ClearScreen("Update Order Status");
            var orders = _orderService.GetAllOrders();

            if (orders.Count == 0)
            {
                ConsoleHelper.WriteWarning("No orders found.");
                ConsoleHelper.PressAnyKey();
                return;
            }

            RenderOrderTable(orders);

            Console.WriteLine();
            Console.Write("  Enter Order ID to update (or 0 to go back): ");
            string idInput = Console.ReadLine()?.Trim() ?? "0";

            if (idInput == "0" || idInput == string.Empty)
                return;

            if (!int.TryParse(idInput, out int orderId))
            {
                ConsoleHelper.WriteError("Please enter a valid order ID.");
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

            if (order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Cancelled)
            {
                ConsoleHelper.WriteError($"Order #{orderId} is {order.Status} and cannot be updated.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            Console.WriteLine();
            ConsoleHelper.WriteInfo($"  Order #{order.Id} — current status: {order.Status}");
            Console.WriteLine();

            string[] statusNames = ["Pending", "Processing", "Shipped", "Delivered", "Cancelled"];
            int choice = MenuRenderer.Show("Select New Status", statusNames);

            var newStatus = (OrderStatus)(choice - 1);

            if (newStatus == order.Status)
            {
                ConsoleHelper.WriteWarning("Status unchanged.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            try
            {
                var updated = _orderService.UpdateOrderStatus(orderId, newStatus);
                Console.WriteLine();
                ConsoleHelper.WriteSuccess($"Order #{updated.Id} status updated to {updated.Status}.");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine();
                ConsoleHelper.WriteError(ex.Message);
            }

            ConsoleHelper.PressAnyKey();
        }
    }

    private void RenderOrderTable(IReadOnlyList<Order> orders)
    {
        Console.WriteLine();
        Console.WriteLine($"  {"ID",-6} {"Customer",-15} {"Date",-12} {"Total",10}  {"Status",-12} Items");
        Console.WriteLine($"  {new string('─', 70)}");

        foreach (var o in orders)
        {
            string username = _userService.FindById(o.UserId)?.Username ?? $"User #{o.UserId}";
            string date     = o.OrderDate.ToLocalTime().ToString("yyyy-MM-dd");
            string items    = string.Join(", ", o.Items.Select(i => $"{i.ProductName} x{i.Quantity}"));

            Console.Write($"  {o.Id,-6} {username,-15} {date,-12} {o.Total,10:C}  ");
            WriteStatus(o.Status);
            Console.WriteLine($"  {items}");
        }

        Console.WriteLine($"  {new string('─', 70)}");
    }

    private static void WriteStatus(OrderStatus status)
    {
        string label = $"{status,-12}";
        switch (status)
        {
            case OrderStatus.Delivered:
                ConsoleHelper.WriteSuccessInline(label); break;
            case OrderStatus.Cancelled:
                ConsoleHelper.WriteErrorInline(label); break;
            case OrderStatus.Shipped:
                ConsoleHelper.WriteInfoInline(label); break;
            default:
                ConsoleHelper.WriteWarningInline(label); break;
        }
    }
}
