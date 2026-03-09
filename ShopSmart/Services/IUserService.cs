namespace ShopSmart.Services;

using ShopSmart.Enums;
using ShopSmart.Models;

public interface IUserService
{
    /// <summary>
    /// Validates input and registers a new user.
    /// Returns the created User on success.
    /// Throws <see cref="ValidationException"/> on invalid or duplicate input.
    /// </summary>
    User Register(string username, string email, string password, UserRole role);

    /// <summary>
    /// Authenticates a user by username and password.
    /// Returns the User on success.
    /// Throws <see cref="ValidationException"/> with a generic message on failure
    /// (never reveals whether the username or password was wrong).
    /// </summary>
    User Login(string username, string password);

    User? FindById(int id);
    User? FindByUsername(string username);

    /// <summary>Adds funds to the user's wallet. Throws <see cref="ValidationException"/> if amount is not positive.</summary>
    void TopUpWallet(User user, decimal amount);
}
