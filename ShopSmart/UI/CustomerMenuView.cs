namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class CustomerMenuView
{
    private readonly IProductService   _productService;
    private readonly BrowseProductsView _browseView;

    // Future: ICartService, IOrderService, IReviewService added here.

    public CustomerMenuView(IProductService productService, BrowseProductsView browseView)
    {
        _productService = productService;
        _browseView     = browseView;
    }

    /// <summary>Runs the customer menu loop. Returns when the user logs out.</summary>
    public void Run(User currentUser)
    {
        while (true)
        {
            string[] options =
            [
                "Browse Products",
                // Issues #4–#13 options will be inserted above Logout.
                "Logout"
            ];

            int choice = MenuRenderer.Show($"Customer Menu – {currentUser.Username}", options);

            switch (choice)
            {
                case 1:
                    _browseView.Run();
                    break;

                case 2: // Logout (always last)
                    ConsoleHelper.WriteInfo("Logged out successfully.");
                    ConsoleHelper.PressAnyKey();
                    return;
            }
        }
    }
}
