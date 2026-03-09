namespace ShopSmart.Services.Payments;

using ShopSmart.Data;
using ShopSmart.Enums;
using ShopSmart.Models;

public class WalletPaymentStrategy : IPaymentStrategy
{
    private readonly UserRepository _userRepo;

    public WalletPaymentStrategy(UserRepository userRepo) => _userRepo = userRepo;

    public string MethodName => "Wallet";

    public PaymentStatus Execute(User user, decimal amount)
    {
        if (user.WalletBalance < amount)
            throw new ValidationException(
                $"Insufficient wallet balance. Balance: {user.WalletBalance:C}, Required: {amount:C}.");

        user.WalletBalance -= amount;
        _userRepo.Save();
        return PaymentStatus.Completed;
    }
}
