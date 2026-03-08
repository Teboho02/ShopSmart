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
    private readonly ViewCartView       _viewCartView;
    private readonly UpdateCartView     _updateCartView;
    private readonly CheckoutView       _checkoutView;
    private readonly WalletBalanceView  _walletBalanceView;
    private readonly AddWalletFundsView _addWalletFundsView;
    private readonly OrderHistoryView   _orderHistoryView;
    private readonly TrackOrderView     _trackOrderView;
    private readonly ReviewProductsView _reviewProductsView;

    public CustomerMenuView(
        IProductService    productService,
        BrowseProductsView browseView,
        SearchProductsView searchView,
        ICartService       cartService,
        AddToCartView      addToCartView,
        ViewCartView       viewCartView,
        UpdateCartView     updateCartView,
        CheckoutView       checkoutView,
        WalletBalanceView  walletBalanceView,
        AddWalletFundsView addWalletFundsView,
        OrderHistoryView   orderHistoryView,
        TrackOrderView     trackOrderView,
        ReviewProductsView reviewProductsView)
    {
        _productService     = productService;
        _browseView         = browseView;
        _searchView         = searchView;
        _cartService        = cartService;
        _addToCartView      = addToCartView;
        _viewCartView       = viewCartView;
        _updateCartView     = updateCartView;
        _checkoutView       = checkoutView;
        _walletBalanceView  = walletBalanceView;
        _addWalletFundsView = addWalletFundsView;
        _orderHistoryView   = orderHistoryView;
        _trackOrderView     = trackOrderView;
        _reviewProductsView = reviewProductsView;
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
                "View Cart",
                "Update Cart",
                "Checkout",
                "View Wallet Balance",
                "Add Wallet Funds",
                "Order History",
                "Track Order",
                "Review Products",
                "Logout"
            ];

            int choice = MenuRenderer.Show($"Customer Menu – {currentUser.Username}", options);

            switch (choice)
            {
                case 1:  _browseView.Run();                        break;
                case 2:  _searchView.Run();                        break;
                case 3:  _addToCartView.Run(currentUser);          break;
                case 4:  _viewCartView.Run(currentUser);           break;
                case 5:  _updateCartView.Run(currentUser);         break;
                case 6:  _checkoutView.Run(currentUser);           break;
                case 7:  _walletBalanceView.Run(currentUser);      break;
                case 8:  _addWalletFundsView.Run(currentUser);     break;
                case 9:  _orderHistoryView.Run(currentUser);       break;
                case 10: _trackOrderView.Run(currentUser);         break;
                case 11: _reviewProductsView.Run(currentUser);     break;

                case 12: // Logout (always last)
                    ConsoleHelper.WriteInfo("Logged out successfully.");
                    ConsoleHelper.PressAnyKey();
                    return;
            }
        }
    }
}
