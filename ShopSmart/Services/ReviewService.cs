namespace ShopSmart.Services;

using ShopSmart.Data;
using ShopSmart.Enums;
using ShopSmart.Models;

public class ReviewService : IReviewService
{
    private readonly OrderRepository  _orderRepo;
    private readonly ReviewRepository _reviewRepo;

    public ReviewService(OrderRepository orderRepo, ReviewRepository reviewRepo)
    {
        _orderRepo  = orderRepo;
        _reviewRepo = reviewRepo;
    }

    public IReadOnlyList<(int ProductId, string ProductName)> GetReviewableProducts(User user)
    {
        return _orderRepo.GetByUser(user.Id)
            .Where(o => o.Status == OrderStatus.Delivered)
            .SelectMany(o => o.Items)
            .DistinctBy(i => i.ProductId)
            .Where(i => !_reviewRepo.HasReviewed(user.Id, i.ProductId))
            .Select(i => (i.ProductId, i.ProductName))
            .ToList()
            .AsReadOnly();
    }

    public Review SubmitReview(User user, int productId, string productName, int rating, string comment)
    {
        if (rating < 1 || rating > 5)
            throw new ValidationException("Rating must be between 1 and 5.");

        bool inDeliveredOrder = _orderRepo.GetByUser(user.Id)
            .Any(o => o.Status == OrderStatus.Delivered &&
                      o.Items.Any(i => i.ProductId == productId));

        if (!inDeliveredOrder)
            throw new ValidationException("You can only review products from delivered orders.");

        if (_reviewRepo.HasReviewed(user.Id, productId))
            throw new ValidationException("You have already reviewed this product.");

        var review = new Review(
            _reviewRepo.NextReviewId(),
            user.Id,
            productId,
            productName,
            rating,
            comment,
            DateTime.UtcNow);

        _reviewRepo.Add(review);
        return review;
    }
}
