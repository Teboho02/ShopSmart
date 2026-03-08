namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class AdminMenuView
{
    private readonly IProductService _productService;
    private readonly AddProductView  _addProductView;

    public AdminMenuView(IProductService productService, AddProductView addProductView)
    {
        _productService = productService;
        _addProductView = addProductView;
    }

    /// <summary>Runs the administrator menu loop. Returns when the user logs out.</summary>
    public void Run(User currentUser)
    {
        while (true)
        {
            string[] options =
            [
                "Add Product",
                "Logout"
            ];

            int choice = MenuRenderer.Show($"Administrator Menu – {currentUser.Username}", options);

            switch (choice)
            {
                case 1: _addProductView.Run(); break;

                case 2: // Logout (always last)
                    ConsoleHelper.WriteInfo("Logged out successfully.");
                    ConsoleHelper.PressAnyKey();
                    return;
            }
        }
    }
}
