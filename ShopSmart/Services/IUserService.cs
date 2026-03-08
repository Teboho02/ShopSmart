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

    User? FindById(int id);
    User? FindByUsername(string username);
}
