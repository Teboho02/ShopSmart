using ShopSmart.Enums;
using ShopSmart.Models;
using ShopSmart.Services;

public class CartServiceTests : TestBase
{
    private User    _user    = null!;
    private Product _product = null!;

    private void Setup()
    {
        _user    = UserService.Register("alice", "alice@test.com", "pass123", UserRole.Customer);
        _product = ProductService.AddProduct("Keyboard", "desc", "Electronics", 89.99m, 10);
    }

    // --- AddToCart ---

    [Fact]
    public void AddToCart_NewItem_AddsToCart()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 2);

        var cart = CartService.GetCart(_user);

        Assert.Single(cart);
        Assert.Equal(_product.Id, cart[0].ProductId);
        Assert.Equal(2, cart[0].Quantity);
    }

    [Fact]
    public void AddToCart_ExistingItem_IncrementsQuantity()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 2);
        CartService.AddToCart(_user, _product.Id, 3);

        var cart = CartService.GetCart(_user);

        Assert.Single(cart);
        Assert.Equal(5, cart[0].Quantity);
    }

    [Fact]
    public void AddToCart_ExceedsStock_ThrowsValidationException()
    {
        Setup();

        Assert.Throws<ValidationException>(() =>
            CartService.AddToCart(_user, _product.Id, 11));
    }

    [Fact]
    public void AddToCart_ZeroQuantity_ThrowsValidationException()
    {
        Setup();

        Assert.Throws<ValidationException>(() =>
            CartService.AddToCart(_user, _product.Id, 0));
    }

    [Fact]
    public void AddToCart_InactiveProduct_ThrowsValidationException()
    {
        Setup();
        ProductService.DeleteProduct(_product.Id);

        Assert.Throws<ValidationException>(() =>
            CartService.AddToCart(_user, _product.Id, 1));
    }

    [Fact]
    public void AddToCart_SnapshotsPriceAtTimeOfAdding()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 1);

        // Change price after adding to cart
        ProductService.UpdateProduct(_product.Id, _product.Name, _product.Description,
                                     _product.Category, 999m, _product.Stock);

        var cart = CartService.GetCart(_user);
        Assert.Equal(89.99m, cart[0].UnitPrice); // still the original price
    }

    // --- UpdateQuantity ---

    [Fact]
    public void UpdateQuantity_SetsNewQuantity()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 2);

        CartService.UpdateQuantity(_user, _product.Id, 5);

        Assert.Equal(5, CartService.GetCart(_user)[0].Quantity);
    }

    [Fact]
    public void UpdateQuantity_ZeroQuantity_RemovesItem()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 2);

        CartService.UpdateQuantity(_user, _product.Id, 0);

        Assert.Empty(CartService.GetCart(_user));
    }

    [Fact]
    public void UpdateQuantity_ExceedsStock_ThrowsValidationException()
    {
        Setup();
        CartService.AddToCart(_user, _product.Id, 1);

        Assert.Throws<ValidationException>(() =>
            CartService.UpdateQuantity(_user, _product.Id, 11));
    }

    // --- GetCart ---

    [Fact]
    public void GetCart_EmptyCart_ReturnsEmptyList()
    {
        Setup();

        Assert.Empty(CartService.GetCart(_user));
    }

    [Fact]
    public void GetCart_MultipleProducts_ReturnsAll()
    {
        Setup();
        var p2 = ProductService.AddProduct("Mouse", "desc", "Electronics", 29.99m, 5);
        CartService.AddToCart(_user, _product.Id, 1);
        CartService.AddToCart(_user, p2.Id, 2);

        Assert.Equal(2, CartService.GetCart(_user).Count);
    }
}
