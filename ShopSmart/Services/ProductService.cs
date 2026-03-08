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

    public IReadOnlyList<Product> SearchActive(string searchTerm) =>
        _repo.GetAllActive()
             .Where(p =>
                 p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)        ||
                 p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                 p.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
             .OrderBy(p => p.Category)
             .ThenBy(p => p.Name)
             .ToList()
             .AsReadOnly();

    public Product AddProduct(string name, string description, string category, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Product name cannot be empty.");
        if (string.IsNullOrWhiteSpace(description))
            throw new ValidationException("Description cannot be empty.");
        if (string.IsNullOrWhiteSpace(category))
            throw new ValidationException("Category cannot be empty.");
        if (price <= 0)
            throw new ValidationException("Price must be greater than zero.");
        if (stock < 0)
            throw new ValidationException("Stock cannot be negative.");

        var product = new Product(
            _repo.NextProductId(),
            name.Trim(),
            description.Trim(),
            category.Trim(),
            price,
            stock);

        _repo.Add(product);
        return product;
    }

    public Product UpdateProduct(int id, string name, string description, string category, decimal price, int stock)
    {
        var product = _repo.FindById(id);
        if (product is null || !product.IsActive)
            throw new ValidationException("Product not found.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Product name cannot be empty.");
        if (string.IsNullOrWhiteSpace(description))
            throw new ValidationException("Description cannot be empty.");
        if (string.IsNullOrWhiteSpace(category))
            throw new ValidationException("Category cannot be empty.");
        if (price <= 0)
            throw new ValidationException("Price must be greater than zero.");
        if (stock < 0)
            throw new ValidationException("Stock cannot be negative.");

        product.Name        = name.Trim();
        product.Description = description.Trim();
        product.Category    = category.Trim();
        product.Price       = price;
        product.Stock       = stock;

        _repo.Save();
        return product;
    }

    public void DeleteProduct(int id)
    {
        var product = _repo.FindById(id);
        if (product is null || !product.IsActive)
            throw new ValidationException("Product not found.");

        product.IsActive = false;
        _repo.Save();
    }
}
