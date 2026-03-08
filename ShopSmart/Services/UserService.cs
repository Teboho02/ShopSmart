namespace ShopSmart.Services;

using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ShopSmart.Data;
using ShopSmart.Enums;
using ShopSmart.Models;

public class UserService : IUserService
{
    private readonly UserRepository _repo;

    public UserService(UserRepository repo) => _repo = repo;

    public User Register(string username, string email, string password, UserRole role)
    {
        ValidateUsername(username);
        ValidateEmail(email);
        ValidatePassword(password);
        EnsureUsernameUnique(username);
        EnsureEmailUnique(email);

        string hash = HashPassword(password);
        var user = new User(
            _repo.NextUserId(),
            username.Trim(),
            email.Trim().ToLowerInvariant(),
            hash,
            role);

        _repo.Add(user);
        return user;
    }

    public User Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new ValidationException("Invalid username or password.");

        var user = _repo.FindByUsername(username);
        if (user is null || user.PasswordHash != HashPassword(password))
            throw new ValidationException("Invalid username or password.");

        return user;
    }

    public User? FindById(int id)            => _repo.FindById(id);
    public User? FindByUsername(string name) => _repo.FindByUsername(name);

    public void TopUpWallet(User user, decimal amount)
    {
        if (amount <= 0)
            throw new ValidationException("Amount must be greater than zero.");
        user.WalletBalance += amount;
        _repo.Save();
    }

    // --- Validators ---

    private static void ValidateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ValidationException("Username cannot be empty.");
        if (username.Trim().Length < 3)
            throw new ValidationException("Username must be at least 3 characters.");
        if (username.Trim().Length > 50)
            throw new ValidationException("Username cannot exceed 50 characters.");
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ValidationException("Email cannot be empty.");
        if (!Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ValidationException("Email address is not valid.");
    }

    private static void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ValidationException("Password cannot be empty.");
        if (password.Length < 6)
            throw new ValidationException("Password must be at least 6 characters.");
    }

    private void EnsureUsernameUnique(string username)
    {
        if (_repo.FindByUsername(username) is not null)
            throw new ValidationException($"Username '{username.Trim()}' is already taken.");
    }

    private void EnsureEmailUnique(string email)
    {
        if (_repo.FindByEmail(email) is not null)
            throw new ValidationException($"An account with email '{email.Trim()}' already exists.");
    }

    // --- Helpers ---

    private static string HashPassword(string password)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
