namespace ShopSmart.Data;

using ShopSmart.Models;

/// <summary>
/// Central in-memory data store. One instance is created at startup and
/// injected into all repositories. Acts as the application's "database".
/// </summary>
public class AppData
{
    public List<User> Users { get; } = [];


    private int _nextUserId = 1;
    public int NextUserId() => _nextUserId++;
}
