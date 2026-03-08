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

    public void UpdateQuantity(User user, int productId, int newQuantity)
    {
        if (newQuantity < 0)
            throw new ValidationException("Quantity cannot be negative.");

        var item = _repo.FindItem(user.Id, productId)
            ?? throw new ValidationException("That product is not in your cart.");

        if (newQuantity == 0)
        {
            _repo.Remove(item);
            return;
        }

        var product = _productService.GetActiveById(productId)
            ?? throw new ValidationException("That product is no longer available.");

        if (newQuantity > product.Stock)
            throw new ValidationException($"Only {product.Stock} unit(s) in stock.");

        item.Quantity = newQuantity;
    }
}
