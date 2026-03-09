namespace ShopSmart.Services.Payments;

using ShopSmart.Data;

/// <summary>
/// Creates payment strategy instances with their required dependencies.
/// Injected into CheckoutView so the UI layer never references repositories directly.
/// </summary>
public class PaymentStrategyFactory
{
    private readonly UserRepository    _userRepo;
    private readonly VoucherRepository _voucherRepo;

    public PaymentStrategyFactory(UserRepository userRepo, VoucherRepository voucherRepo)
    {
        _userRepo    = userRepo;
        _voucherRepo = voucherRepo;
    }

    public IPaymentStrategy Wallet()             => new WalletPaymentStrategy(_userRepo);
    public IPaymentStrategy Eft()                => new EftPaymentStrategy();
    public IPaymentStrategy PayPal(string email) => new PayPalPaymentStrategy(email);
    public IPaymentStrategy Voucher(string code) => new VoucherPaymentStrategy(code, _voucherRepo, _userRepo);
}
