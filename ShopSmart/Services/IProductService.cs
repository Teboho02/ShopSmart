namespace ShopSmart.Services;

using ShopSmart.Models;

public interface IProductService
{
    /// <summary>Returns all active (non-deleted) products, ordered by category then name.</summary>
    IReadOnlyList<Product> GetAllActive();

    /// <summary>Returns an active product by ID, or null if not found or soft-deleted.</summary>
    Product? GetActiveById(int id);
}
