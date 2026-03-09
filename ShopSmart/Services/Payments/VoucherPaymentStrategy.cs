namespace ShopSmart.Services.Payments;

using ShopSmart.Data;
using ShopSmart.Enums;
using ShopSmart.Models;

public class VoucherPaymentStrategy : IPaymentStrategy
{
    private readonly string            _code;
    private readonly VoucherRepository _voucherRepo;
    private readonly UserRepository    _userRepo;

    public VoucherPaymentStrategy(string code, VoucherRepository voucherRepo, UserRepository userRepo)
    {
        _code        = code;
        _voucherRepo = voucherRepo;
        _userRepo    = userRepo;
    }

    public string MethodName => $"Voucher ({_code.ToUpperInvariant()})";

    public PaymentStatus Execute(User user, decimal amount)
    {
        var voucher = _voucherRepo.FindByCode(_code)
            ?? throw new ValidationException("Invalid voucher code.");

        if (voucher.IsRedeemed)
            throw new ValidationException("This voucher has already been used.");

        decimal remainder = amount - voucher.Value;

        if (remainder > 0)
        {
            // Voucher covers part of the amount — wallet pays the rest.
            if (user.WalletBalance < remainder)
                throw new ValidationException(
                    $"Voucher covers {voucher.Value:C}. " +
                    $"Remaining {remainder:C} cannot be covered by your wallet balance ({user.WalletBalance:C}).");

            user.WalletBalance -= remainder;
            _userRepo.Save();
        }

        voucher.IsRedeemed = true;
        _voucherRepo.Save();

        return PaymentStatus.Completed;
    }
}
