namespace ShopSmart.Services;

using ShopSmart.Models;

public interface IProductService
{
    /// <summary>Returns all active (non-deleted) products, ordered by category then name.</summary>
    IReadOnlyList<Product> GetAllActive();

    /// <summary>Returns an active product by ID, or null if not found or soft-deleted.</summary>
    Product? GetActiveById(int id);

    /// <summary>
    /// Returns all active products whose Name, Description, or Category contains
    /// the search term (case-insensitive), ordered by category then name.
    /// Returns an empty list when nothing matches.
    /// </summary>
    IReadOnlyList<Product> SearchActive(string searchTerm);

    /// <summary>
    /// Validates inputs and creates a new active product.
    /// Throws <see cref="ValidationException"/> on invalid input.
    /// </summary>
    Product AddProduct(string name, string description, string category, decimal price, int stock);
}
