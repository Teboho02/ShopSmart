using ShopSmart.Services;

public class ProductServiceTests : TestBase
{
    // --- AddProduct ---

    [Fact]
    public void AddProduct_ValidInput_ReturnsActiveProduct()
    {
        var p = ProductService.AddProduct("Keyboard", "Mech keyboard", "Electronics", 89.99m, 10);

        Assert.Equal("Keyboard", p.Name);
        Assert.Equal("Electronics", p.Category);
        Assert.Equal(89.99m, p.Price);
        Assert.Equal(10, p.Stock);
        Assert.True(p.IsActive);
    }

    [Fact]
    public void AddProduct_EmptyName_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() =>
            ProductService.AddProduct("", "desc", "cat", 10m, 5));
    }

    [Fact]
    public void AddProduct_EmptyCategory_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() =>
            ProductService.AddProduct("Item", "desc", "", 10m, 5));
    }

    [Fact]
    public void AddProduct_ZeroPrice_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() =>
            ProductService.AddProduct("Item", "desc", "cat", 0m, 5));
    }

    [Fact]
    public void AddProduct_NegativePrice_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() =>
            ProductService.AddProduct("Item", "desc", "cat", -1m, 5));
    }

    [Fact]
    public void AddProduct_NegativeStock_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() =>
            ProductService.AddProduct("Item", "desc", "cat", 10m, -1));
    }

    [Fact]
    public void AddProduct_ZeroStock_IsAllowed()
    {
        var p = ProductService.AddProduct("Out of stock item", "desc", "cat", 10m, 0);

        Assert.Equal(0, p.Stock);
    }

    // --- GetAllActive / GetAll ---

    [Fact]
    public void GetAllActive_ExcludesDeletedProducts()
    {
        ProductService.AddProduct("Active",  "desc", "cat", 10m, 5);
        var deleted = ProductService.AddProduct("Deleted", "desc", "cat", 10m, 5);
        ProductService.DeleteProduct(deleted.Id);

        var active = ProductService.GetAllActive();

        Assert.Single(active);
        Assert.Equal("Active", active[0].Name);
    }

    [Fact]
    public void GetAll_IncludesDeletedProducts()
    {
        ProductService.AddProduct("Active",  "desc", "cat", 10m, 5);
        var deleted = ProductService.AddProduct("Deleted", "desc", "cat", 10m, 5);
        ProductService.DeleteProduct(deleted.Id);

        var all = ProductService.GetAll();

        Assert.Equal(2, all.Count);
    }

    [Fact]
    public void GetAllActive_OrdersByCategoryThenName()
    {
        ProductService.AddProduct("Zebra Shirt", "desc", "Clothing",     10m, 5);
        ProductService.AddProduct("Apple Phone", "desc", "Electronics",  10m, 5);
        ProductService.AddProduct("Ant Shirt",   "desc", "Clothing",     10m, 5);

        var active = ProductService.GetAllActive();

        Assert.Equal("Ant Shirt",   active[0].Name);
        Assert.Equal("Zebra Shirt", active[1].Name);
        Assert.Equal("Apple Phone", active[2].Name);
    }

    // --- GetActiveById ---

    [Fact]
    public void GetActiveById_ExistingId_ReturnsProduct()
    {
        var p = ProductService.AddProduct("Keyboard", "desc", "Electronics", 89.99m, 5);

        var found = ProductService.GetActiveById(p.Id);

        Assert.NotNull(found);
        Assert.Equal("Keyboard", found.Name);
    }

    [Fact]
    public void GetActiveById_DeletedProduct_ReturnsNull()
    {
        var p = ProductService.AddProduct("Keyboard", "desc", "Electronics", 89.99m, 5);
        ProductService.DeleteProduct(p.Id);

        Assert.Null(ProductService.GetActiveById(p.Id));
    }

    // --- SearchActive ---

    [Fact]
    public void SearchActive_MatchesByName()
    {
        ProductService.AddProduct("Wireless Headphones", "desc", "Electronics", 49.99m, 10);
        ProductService.AddProduct("Running Shoes",       "desc", "Clothing",    59.99m, 10);

        var results = ProductService.SearchActive("headphone");

        Assert.Single(results);
        Assert.Equal("Wireless Headphones", results[0].Name);
    }

    [Fact]
    public void SearchActive_MatchesByCategory()
    {
        ProductService.AddProduct("T-Shirt",    "desc", "Clothing",     14.99m, 5);
        ProductService.AddProduct("USB-C Hub",  "desc", "Electronics",  29.99m, 5);

        var results = ProductService.SearchActive("clothing");

        Assert.Single(results);
    }

    [Fact]
    public void SearchActive_IsCaseInsensitive()
    {
        ProductService.AddProduct("Wireless Headphones", "desc", "Electronics", 49.99m, 10);

        Assert.Single(ProductService.SearchActive("HEADPHONE"));
        Assert.Single(ProductService.SearchActive("headphone"));
    }

    // --- UpdateProduct ---

    [Fact]
    public void UpdateProduct_ChangesAllFields()
    {
        var p = ProductService.AddProduct("Old", "old desc", "OldCat", 10m, 5);

        var updated = ProductService.UpdateProduct(p.Id, "New", "new desc", "NewCat", 20m, 8);

        Assert.Equal("New",     updated.Name);
        Assert.Equal("new desc", updated.Description);
        Assert.Equal("NewCat",  updated.Category);
        Assert.Equal(20m,       updated.Price);
        Assert.Equal(8,         updated.Stock);
    }

    [Fact]
    public void UpdateProduct_DeletedProduct_ThrowsValidationException()
    {
        var p = ProductService.AddProduct("Keyboard", "desc", "Electronics", 89.99m, 5);
        ProductService.DeleteProduct(p.Id);

        Assert.Throws<ValidationException>(() =>
            ProductService.UpdateProduct(p.Id, "New", "desc", "cat", 10m, 5));
    }

    // --- DeleteProduct ---

    [Fact]
    public void DeleteProduct_SetsIsActiveToFalse()
    {
        var p = ProductService.AddProduct("Keyboard", "desc", "Electronics", 89.99m, 10);

        ProductService.DeleteProduct(p.Id);

        Assert.False(p.IsActive);
    }

    [Fact]
    public void DeleteProduct_AlreadyDeleted_ThrowsValidationException()
    {
        var p = ProductService.AddProduct("Keyboard", "desc", "Electronics", 89.99m, 10);
        ProductService.DeleteProduct(p.Id);

        Assert.Throws<ValidationException>(() => ProductService.DeleteProduct(p.Id));
    }

    // --- RestockProduct ---

    [Fact]
    public void RestockProduct_AddsToExistingStock()
    {
        var p = ProductService.AddProduct("Keyboard", "desc", "Electronics", 89.99m, 5);

        var updated = ProductService.RestockProduct(p.Id, 10);

        Assert.Equal(15, updated.Stock);
    }

    [Fact]
    public void RestockProduct_ZeroAmount_ThrowsValidationException()
    {
        var p = ProductService.AddProduct("Keyboard", "desc", "Electronics", 89.99m, 5);

        Assert.Throws<ValidationException>(() => ProductService.RestockProduct(p.Id, 0));
    }

    [Fact]
    public void RestockProduct_NegativeAmount_ThrowsValidationException()
    {
        var p = ProductService.AddProduct("Keyboard", "desc", "Electronics", 89.99m, 5);

        Assert.Throws<ValidationException>(() => ProductService.RestockProduct(p.Id, -5));
    }

    [Fact]
    public void RestockProduct_DeletedProduct_ThrowsValidationException()
    {
        var p = ProductService.AddProduct("Keyboard", "desc", "Electronics", 89.99m, 5);
        ProductService.DeleteProduct(p.Id);

        Assert.Throws<ValidationException>(() => ProductService.RestockProduct(p.Id, 10));
    }
}
