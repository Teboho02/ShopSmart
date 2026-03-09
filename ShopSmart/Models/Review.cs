namespace ShopSmart.Models;

using System.Text.Json.Serialization;

public class Review
{
    public int      Id          { get; init; }
    public int      UserId      { get; init; }
    public int      ProductId   { get; init; }
    public string   ProductName { get; init; }   // snapshot at review time
    public int      Rating      { get; init; }   // 1–5
    public string   Comment     { get; init; }   // "" if no comment
    public DateTime ReviewedAt  { get; init; }

    [JsonConstructor]
    public Review(int id, int userId, int productId, string productName,
                  int rating, string comment, DateTime reviewedAt)
    {
        Id          = id;
        UserId      = userId;
        ProductId   = productId;
        ProductName = productName;
        Rating      = rating;
        Comment     = comment;
        ReviewedAt  = reviewedAt;
    }
}
