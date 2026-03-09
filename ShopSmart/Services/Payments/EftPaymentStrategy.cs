namespace ShopSmart.Services.Payments;

using ShopSmart.Enums;
using ShopSmart.Models;

public class EftPaymentStrategy : IPaymentStrategy
{
    public string MethodName => "EFT";

    public PaymentStatus Execute(User user, decimal amount)
    {
        // EFT is confirmed externally — no deduction from the system.
        return PaymentStatus.Pending;
    }
}
