namespace ShopSmart.Data;

using ShopSmart.Models;

public class UserRepository
{
    private static readonly string FilePath =
        Path.Combine(AppContext.BaseDirectory, "data", "users.json");

    private readonly AppData _data;
    private int _nextId = 1;

    public UserRepository(AppData data)
    {
        _data = data;

        var loaded = JsonFileStore.Load<User>(FilePath);
        _data.Users.AddRange(loaded);

        if (loaded.Count > 0)
            _nextId = loaded.Max(u => u.Id) + 1;
    }

    public void Add(User user)
    {
        _data.Users.Add(user);
        JsonFileStore.Save(FilePath, _data.Users);
    }

    public User? FindById(int id) =>
        _data.Users.FirstOrDefault(u => u.Id == id);

    public User? FindByUsername(string username) =>
        _data.Users.FirstOrDefault(u =>
            string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));

    public User? FindByEmail(string email) =>
        _data.Users.FirstOrDefault(u =>
            string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));

    public IReadOnlyList<User> GetAll() =>
        _data.Users.AsReadOnly();

    public int NextUserId() => _nextId++;
}
