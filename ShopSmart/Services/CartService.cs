namespace ShopSmart.Services;

using ShopSmart.Data;
using ShopSmart.Models;

public class CartService : ICartService
{
    private readonly CartRepository  _repo;
    private readonly IProductService _productService;

    public CartService(CartRepository repo, IProductService productService)
    {
        _repo           = repo;
        _productService = productService;
    }

    public void AddToCart(User user, int productId, int quantity)
    {
        if (quantity < 1)
            throw new ValidationException("Quantity must be at least 1.");

        var product = _productService.GetActiveById(productId)
            ?? throw new ValidationException($"Product with ID {productId} not found.");

        int existingQty  = _repo.FindItem(user.Id, productId)?.Quantity ?? 0;
        int available    = product.Stock - existingQty;

        if (quantity > available)
            throw new ValidationException(
                available > 0
                    ? $"Only {available} unit(s) available (you already have {existingQty} in your cart)."
                    : "This product is out of stock.");

        var existing = _repo.FindItem(user.Id, productId);
        if (existing is not null)
            existing.Quantity += quantity;
        else
            _repo.Add(new CartItem(user.Id, productId, product.Name, product.Price, quantity));
    }

    public IReadOnlyList<CartItem> GetCart(User user) =>
        _repo.GetByUser(user.Id);
}
