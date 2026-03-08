namespace ShopSmart.Data;

using ShopSmart.Models;

public class ReviewRepository
{
    private static readonly string FilePath =
        Path.Combine(AppContext.BaseDirectory, "data", "reviews.json");

    private readonly AppData _data;
    private int _nextId = 1;

    public ReviewRepository(AppData data)
    {
        _data = data;

        var loaded = JsonFileStore.Load<Review>(FilePath);
        _data.Reviews.AddRange(loaded);

        if (loaded.Count > 0)
            _nextId = loaded.Max(r => r.Id) + 1;
    }

    public void Add(Review review)
    {
        _data.Reviews.Add(review);
        JsonFileStore.Save(FilePath, _data.Reviews);
    }

    public IReadOnlyList<Review> GetByUser(int userId) =>
        _data.Reviews.Where(r => r.UserId == userId).ToList().AsReadOnly();

    public bool HasReviewed(int userId, int productId) =>
        _data.Reviews.Any(r => r.UserId == userId && r.ProductId == productId);

    public int NextReviewId() => _nextId++;
}
