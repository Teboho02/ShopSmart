namespace ShopSmart.Data;

using ShopSmart.Models;

public class ProductRepository
{
    private static readonly string FilePath =
        Path.Combine(AppContext.BaseDirectory, "data", "products.json");

    private readonly AppData _data;
    private int _nextId = 1;

    public ProductRepository(AppData data)
    {
        _data = data;

        var loaded = JsonFileStore.Load<Product>(FilePath);
        _data.Products.AddRange(loaded);

        if (loaded.Count > 0)
            _nextId = loaded.Max(p => p.Id) + 1;
    }

    public void Add(Product product)
    {
        _data.Products.Add(product);
        JsonFileStore.Save(FilePath, _data.Products);
    }

    public Product? FindById(int id) =>
        _data.Products.FirstOrDefault(p => p.Id == id);

    public IReadOnlyList<Product> GetAll() =>
        _data.Products.AsReadOnly();

    public IReadOnlyList<Product> GetAllActive() =>
        _data.Products.Where(p => p.IsActive).ToList().AsReadOnly();

    public int NextProductId() => _nextId++;

    /// <summary>Persists the current product list (e.g. after a stock change).</summary>
    public void Save() => JsonFileStore.Save(FilePath, _data.Products);
}
