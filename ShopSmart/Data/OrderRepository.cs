namespace ShopSmart.Data;

using ShopSmart.Models;

public class OrderRepository
{
    private static readonly string FilePath =
        Path.Combine(AppContext.BaseDirectory, "data", "orders.json");

    private readonly AppData _data;
    private int _nextId = 1;

    public OrderRepository(AppData data)
    {
        _data = data;

        var loaded = JsonFileStore.Load<Order>(FilePath);
        _data.Orders.AddRange(loaded);

        if (loaded.Count > 0)
            _nextId = loaded.Max(o => o.Id) + 1;
    }

    public void Add(Order order)
    {
        _data.Orders.Add(order);
        JsonFileStore.Save(FilePath, _data.Orders);
    }

    public Order? FindById(int id) =>
        _data.Orders.FirstOrDefault(o => o.Id == id);

    public IReadOnlyList<Order> GetByUser(int userId) =>
        _data.Orders.Where(o => o.UserId == userId).ToList().AsReadOnly();

    public IReadOnlyList<Order> GetAll() =>
        _data.Orders.AsReadOnly();

    public void Save() => JsonFileStore.Save(FilePath, _data.Orders);

    public int NextOrderId() => _nextId++;
}
