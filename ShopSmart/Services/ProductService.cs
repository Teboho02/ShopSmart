namespace ShopSmart.Services;

using ShopSmart.Data;
using ShopSmart.Models;

public class ProductService : IProductService
{
    private readonly ProductRepository _repo;

    public ProductService(ProductRepository repo) => _repo = repo;

    public IReadOnlyList<Product> GetAllActive() =>
        _repo.GetAllActive()
             .OrderBy(p => p.Category)
             .ThenBy(p => p.Name)
             .ToList()
             .AsReadOnly();

    public Product? GetActiveById(int id) =>
        _repo.GetAllActive().FirstOrDefault(p => p.Id == id);
}
