namespace ShopSmart.Models;

using ShopSmart.Enums;

public class User
{
    public int      Id           { get; init; }
    public string   Username     { get; init; }
    public string   Email        { get; init; }
    public string   PasswordHash { get; private set; }
    public UserRole Role         { get; init; }
    public DateTime CreatedAt    { get; init; }

    public User(int id, string username, string email, string passwordHash, UserRole role)
    {
        Id           = id;
        Username     = username;
        Email        = email;
        PasswordHash = passwordHash;
        Role         = role;
        CreatedAt    = DateTime.UtcNow;
    }
}
