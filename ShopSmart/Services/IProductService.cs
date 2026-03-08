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

    /// <summary>Updates an existing active product. Throws <see cref="ValidationException"/> if not found or input invalid.</summary>
    Product UpdateProduct(int id, string name, string description, string category, decimal price, int stock);

    /// <summary>Soft-deletes a product by marking it inactive. Throws <see cref="ValidationException"/> if not found.</summary>
    void DeleteProduct(int id);

    /// <summary>Adds units to an active product's stock. Throws <see cref="ValidationException"/> if not found or amount ≤ 0.</summary>
    Product RestockProduct(int id, int additionalStock);

    /// <summary>Returns all products including inactive ones, ordered by category then name.</summary>
    IReadOnlyList<Product> GetAll();
}
