namespace ShopSmart.Services.Payments;

using ShopSmart.Enums;
using ShopSmart.Models;

public class PayPalPaymentStrategy : IPaymentStrategy
{
    private readonly string _email;

    public PayPalPaymentStrategy(string email) => _email = email;

    public string MethodName => $"PayPal ({_email})";

    public PaymentStatus Execute(User user, decimal amount)
    {
        if (!_email.Contains('@'))
            throw new ValidationException("Please enter a valid PayPal email address.");

        // Simulated PayPal authorisation — always succeeds for valid email.
        return PaymentStatus.Completed;
    }
}
