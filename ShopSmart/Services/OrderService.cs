namespace ShopSmart.Services;

using ShopSmart.Data;
using ShopSmart.Enums;
using ShopSmart.Models;

public class OrderService : IOrderService
{
    private readonly CartRepository     _cartRepo;
    private readonly IProductService    _productService;
    private readonly ProductRepository  _productRepo;
    private readonly OrderRepository    _orderRepo;
    private readonly PaymentRepository  _paymentRepo;
    private readonly UserRepository     _userRepo;

    public OrderService(
        CartRepository    cartRepo,
        IProductService   productService,
        ProductRepository productRepo,
        OrderRepository   orderRepo,
        PaymentRepository paymentRepo,
        UserRepository    userRepo)
    {
        _cartRepo       = cartRepo;
        _productService = productService;
        _productRepo    = productRepo;
        _orderRepo      = orderRepo;
        _paymentRepo    = paymentRepo;
        _userRepo       = userRepo;
    }

    public Order Checkout(User user)
    {
        var cartItems = _cartRepo.GetByUser(user.Id);

        if (cartItems.Count == 0)
            throw new ValidationException("Your cart is empty.");

        decimal total = cartItems.Sum(i => i.UnitPrice * i.Quantity);

        if (user.WalletBalance < total)
            throw new ValidationException(
                $"Insufficient wallet balance. Balance: {user.WalletBalance:C}, Order total: {total:C}.");

        // Re-validate stock and reduce it
        foreach (var item in cartItems)
        {
            var product = _productService.GetActiveById(item.ProductId)
                ?? throw new ValidationException($"'{item.ProductName}' is no longer available.");

            if (product.Stock < item.Quantity)
                throw new ValidationException(
                    $"Insufficient stock for '{item.ProductName}'. Available: {product.Stock}.");

            product.Stock -= item.Quantity;
        }
        _productRepo.Save();

        // Create Order
        var orderItems = cartItems
            .Select(i => new OrderItem(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity))
            .ToList();

        var order = new Order(
            _orderRepo.NextOrderId(),
            user.Id,
            DateTime.UtcNow,
            OrderStatus.Pending,
            orderItems,
            total,
            "Wallet");

        _orderRepo.Add(order);

        // Create Payment
        var payment = new Payment(
            _paymentRepo.NextPaymentId(),
            order.Id,
            total,
            "Wallet",
            PaymentStatus.Completed,
            DateTime.UtcNow);

        _paymentRepo.Add(payment);

        // Deduct wallet and persist
        user.WalletBalance -= total;
        _userRepo.Save();

        // Clear cart
        _cartRepo.Clear(user.Id);

        return order;
    }

    public IReadOnlyList<Order> GetOrderHistory(int userId) =>
        _orderRepo.GetByUser(userId)
                  .OrderByDescending(o => o.OrderDate)
                  .ToList()
                  .AsReadOnly();

    public Payment? GetPaymentForOrder(int orderId) =>
        _paymentRepo.FindByOrderId(orderId);
}
