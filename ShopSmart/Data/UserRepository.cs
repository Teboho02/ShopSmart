namespace ShopSmart.Data;

using ShopSmart.Models;

public class UserRepository
{
    private readonly AppData _data;

    public UserRepository(AppData data) => _data = data;

    public void Add(User user) =>
        _data.Users.Add(user);

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

    public int NextUserId() => _data.NextUserId();
}
