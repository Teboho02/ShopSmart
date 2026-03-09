namespace ShopSmart.Services.Payments;

using ShopSmart.Enums;
using ShopSmart.Models;

public interface IPaymentStrategy
{
    /// <summary>The display name recorded on the Order and Payment records.</summary>
    string MethodName { get; }

    /// <summary>
    /// Validates and executes the payment.
    /// Returns the resulting <see cref="PaymentStatus"/>.
    /// Throws <see cref="ValidationException"/> if the payment cannot be processed.
    /// </summary>
    PaymentStatus Execute(User user, decimal amount);
}
