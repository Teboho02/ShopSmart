namespace ShopSmart.Services;

using ShopSmart.Models;

public interface IReviewService
{
    /// <summary>
    /// Returns distinct (ProductId, ProductName) pairs from the user's Delivered orders
    /// that have not yet been reviewed by this user.
    /// </summary>
    IReadOnlyList<(int ProductId, string ProductName)> GetReviewableProducts(User user);

    /// <summary>
    /// Validates and saves a review. Throws <see cref="ValidationException"/> if:
    /// rating is not in [1,5], product is not in a delivered order, or already reviewed.
    /// </summary>
    Review SubmitReview(User user, int productId, string productName, int rating, string comment);
}
