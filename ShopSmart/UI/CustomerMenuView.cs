namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class CustomerMenuView
{
    private readonly IProductService    _productService;
    private readonly BrowseProductsView _browseView;
    private readonly SearchProductsView _searchView;
    private readonly ICartService       _cartService;
    private readonly AddToCartView      _addToCartView;

    // Future: IOrderService, IReviewService added here.

    public CustomerMenuView(
        IProductService    productService,
        BrowseProductsView browseView,
        SearchProductsView searchView,
        ICartService       cartService,
        AddToCartView      addToCartView)
    {
        _productService = productService;
        _browseView     = browseView;
        _searchView     = searchView;
        _cartService    = cartService;
        _addToCartView  = addToCartView;
    }

    /// <summary>Runs the customer menu loop. Returns when the user logs out.</summary>
    public void Run(User currentUser)
    {
        while (true)
        {
            string[] options =
            [
                "Browse Products",
                "Search Products",
                "Add to Cart",
                // Issues #6–#13 options will be inserted above Logout.
                "Logout"
            ];

            int choice = MenuRenderer.Show($"Customer Menu – {currentUser.Username}", options);

            switch (choice)
            {
                case 1:
                    _browseView.Run();
                    break;

                case 2:
                    _searchView.Run();
                    break;

                case 3:
                    _addToCartView.Run(currentUser);
                    break;

                case 4: // Logout (always last)
                    ConsoleHelper.WriteInfo("Logged out successfully.");
                    ConsoleHelper.PressAnyKey();
                    return;
            }
        }
    }
}
