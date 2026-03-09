using ShopSmart.Enums;
using ShopSmart.Models;
using ShopSmart.Services;
using ShopSmart.Services.Payments;

public class OrderServiceTests : TestBase
{
    private User    _user    = null!;
    private Product _product = null!;

    private void Setup(decimal walletBalance = 500m)
    {
        _user = UserService.Register("alice", "alice@test.com", "pass123", UserRole.Customer);
        _user.WalletBalance = walletBalance;
        UserRepo.Save();
        _product = ProductService.AddProduct("Keyboard", "desc", "Electronics", 89.99m, 10);
    }

    // ─── Checkout ────────────────────────────────────────────────────────────

    [Fact]
    public void Checkout_EmptyCart_ThrowsValidationException()
    {
        Setup();

        Assert.Throws<ValidationException>(() =>
            OrderService.Checkout(_user, new WalletPaymentStrategy(UserRepo)));
    }

    [Fact]
    public void Checkout_Wallet_CreatesOrderWithCorrectTotal()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 2);

        var order = OrderService.Checkout(_user, new WalletPaymentStrategy(UserRepo));

        Assert.Equal(89.99m * 2, order.Total);
        Assert.Equal("Wallet", order.PaymentMethod);
        Assert.Equal(OrderStatus.Pending, order.Status);
    }

    [Fact]
    public void Checkout_Wallet_DeductsFromBalance()
    {
        Setup(walletBalance: 500m);
        CartService.AddToCart(_user, _product.Id, 2);

        OrderService.Checkout(_user, new WalletPaymentStrategy(UserRepo));

        Assert.Equal(500m - 89.99m * 2, _user.WalletBalance);
    }

    [Fact]
    public void Checkout_Wallet_InsufficientBalance_ThrowsAndLeavesStockUntouched()
    {
        Setup(walletBalance: 10m);
        CartService.AddToCart(_user, _product.Id, 1);

        Assert.Throws<ValidationException>(() =>
            OrderService.Checkout(_user, new WalletPaymentStrategy(UserRepo)));

        Assert.Equal(10, ProductService.GetActiveById(_product.Id)!.Stock); // stock unchanged
    }

    [Fact]
    public void Checkout_Eft_CreatesPendingPayment()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 1);

        var order = OrderService.Checkout(_user, new EftPaymentStrategy());
        var payment = OrderService.GetPaymentForOrder(order.Id);

        Assert.Equal(PaymentStatus.Pending, payment!.Status);
        Assert.Equal("EFT", payment.Method);
    }

    [Fact]
    public void Checkout_Eft_DoesNotDeductWallet()
    {
        Setup(walletBalance: 500m);
        CartService.AddToCart(_user, _product.Id, 1);

        OrderService.Checkout(_user, new EftPaymentStrategy());

        Assert.Equal(500m, _user.WalletBalance);
    }

    [Fact]
    public void Checkout_ReducesProductStock()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 3);

        OrderService.Checkout(_user, new EftPaymentStrategy());

        Assert.Equal(7, ProductService.GetActiveById(_product.Id)!.Stock);
    }

    [Fact]
    public void Checkout_ClearsCart()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 1);

        OrderService.Checkout(_user, new EftPaymentStrategy());

        Assert.Empty(CartService.GetCart(_user));
    }

    [Fact]
    public void Checkout_PayPal_CreatesCompletedPaymentWithEmail()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 1);

        var order = OrderService.Checkout(_user, new PayPalPaymentStrategy("alice@paypal.com"));
        var payment = OrderService.GetPaymentForOrder(order.Id);

        Assert.Equal(PaymentStatus.Completed, payment!.Status);
        Assert.Contains("alice@paypal.com", payment.Method);
    }

    // ─── GetOrderHistory ─────────────────────────────────────────────────────

    [Fact]
    public void GetOrderHistory_ReturnsOnlyUserOrders()
    {
        Setup();
        var bob = UserService.Register("bob", "bob@test.com", "pass123", UserRole.Customer);
        bob.WalletBalance = 500m;

        CartService.AddToCart(_user, _product.Id, 1);
        OrderService.Checkout(_user, new EftPaymentStrategy());

        var p2 = ProductService.AddProduct("Mouse", "desc", "Electronics", 29.99m, 5);
        CartService.AddToCart(bob, p2.Id, 1);
        OrderService.Checkout(bob, new EftPaymentStrategy());

        var history = OrderService.GetOrderHistory(_user.Id);

        Assert.Single(history);
        Assert.Equal(_user.Id, history[0].UserId);
    }

    [Fact]
    public void GetOrderHistory_ReturnsMostRecentFirst()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 1);
        OrderService.Checkout(_user, new EftPaymentStrategy());

        var p2 = ProductService.AddProduct("Mouse", "desc", "Electronics", 29.99m, 5);
        CartService.AddToCart(_user, p2.Id, 1);
        OrderService.Checkout(_user, new EftPaymentStrategy());

        var history = OrderService.GetOrderHistory(_user.Id);

        Assert.True(history[0].OrderDate >= history[1].OrderDate);
    }

    // ─── AdvanceOrderStatus ──────────────────────────────────────────────────

    [Fact]
    public void AdvanceOrderStatus_PendingToProcessing()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 1);
        var order = OrderService.Checkout(_user, new EftPaymentStrategy());

        var advanced = OrderService.AdvanceOrderStatus(order.Id);

        Assert.Equal(OrderStatus.Processing, advanced.Status);
    }

    [Fact]
    public void AdvanceOrderStatus_FullSequence_PendingToDelivered()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 1);
        var order = OrderService.Checkout(_user, new EftPaymentStrategy());

        OrderService.AdvanceOrderStatus(order.Id); // → Processing
        OrderService.AdvanceOrderStatus(order.Id); // → Shipped
        var delivered = OrderService.AdvanceOrderStatus(order.Id); // → Delivered

        Assert.Equal(OrderStatus.Delivered, delivered.Status);
    }

    [Fact]
    public void AdvanceOrderStatus_Delivered_ThrowsValidationException()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 1);
        var order = OrderService.Checkout(_user, new EftPaymentStrategy());

        OrderService.AdvanceOrderStatus(order.Id);
        OrderService.AdvanceOrderStatus(order.Id);
        OrderService.AdvanceOrderStatus(order.Id); // → Delivered

        Assert.Throws<ValidationException>(() =>
            OrderService.AdvanceOrderStatus(order.Id));
    }

    // ─── CancelOrder ─────────────────────────────────────────────────────────

    [Fact]
    public void CancelOrder_PendingOrder_SetsStatusCancelled()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 1);
        var order = OrderService.Checkout(_user, new EftPaymentStrategy());

        var cancelled = OrderService.CancelOrder(order.Id);

        Assert.Equal(OrderStatus.Cancelled, cancelled.Status);
    }

    [Fact]
    public void CancelOrder_DeliveredOrder_ThrowsValidationException()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 1);
        var order = OrderService.Checkout(_user, new EftPaymentStrategy());

        OrderService.AdvanceOrderStatus(order.Id);
        OrderService.AdvanceOrderStatus(order.Id);
        OrderService.AdvanceOrderStatus(order.Id); // → Delivered

        Assert.Throws<ValidationException>(() => OrderService.CancelOrder(order.Id));
    }

    [Fact]
    public void CancelOrder_AlreadyCancelled_ThrowsValidationException()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 1);
        var order = OrderService.Checkout(_user, new EftPaymentStrategy());

        OrderService.CancelOrder(order.Id);

        Assert.Throws<ValidationException>(() => OrderService.CancelOrder(order.Id));
    }
}
