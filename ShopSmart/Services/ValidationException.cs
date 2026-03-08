namespace ShopSmart.Services;

/// <summary>
/// Thrown by service layer methods when user-supplied input fails validation.
/// The Message property always contains a human-readable, UI-displayable string.
/// </summary>
public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}
