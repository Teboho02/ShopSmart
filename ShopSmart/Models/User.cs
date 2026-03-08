namespace ShopSmart.Models;

using System.Text.Json.Serialization;
using ShopSmart.Enums;

public class User
{
    public int      Id            { get; init; }
    public string   Username      { get; init; }
    public string   Email         { get; init; }
    public string   PasswordHash  { get; private set; }
    public UserRole Role          { get; init; }
    public decimal  WalletBalance { get; set; }
    public DateTime CreatedAt     { get; init; }

    /// <summary>Full constructor used by System.Text.Json to restore persisted users.</summary>
    [JsonConstructor]
    public User(int id, string username, string email, string passwordHash,
                UserRole role, decimal walletBalance, DateTime createdAt)
    {
        Id            = id;
        Username      = username;
        Email         = email;
        PasswordHash  = passwordHash;
        Role          = role;
        WalletBalance = walletBalance;
        CreatedAt     = createdAt;
    }

    /// <summary>Convenience constructor for creating new users; wallet starts at 0.</summary>
    public User(int id, string username, string email, string passwordHash, UserRole role)
        : this(id, username, email, passwordHash, role, walletBalance: 0m, createdAt: DateTime.UtcNow) { }
}
