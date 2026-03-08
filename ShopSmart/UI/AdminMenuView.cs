namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class AdminMenuView
{
    private readonly IProductService   _productService;
    private readonly AddProductView    _addProductView;
    private readonly UpdateProductView _updateProductView;
    private readonly DeleteProductView _deleteProductView;

    public AdminMenuView(
        IProductService   productService,
        AddProductView    addProductView,
        UpdateProductView updateProductView,
        DeleteProductView deleteProductView)
    {
        _productService    = productService;
        _addProductView    = addProductView;
        _updateProductView = updateProductView;
        _deleteProductView = deleteProductView;
    }

    /// <summary>Runs the administrator menu loop. Returns when the user logs out.</summary>
    public void Run(User currentUser)
    {
        while (true)
        {
            string[] options =
            [
                "Add Product",
                "Update Product",
                "Delete Product",
                "Logout"
            ];

            int choice = MenuRenderer.Show($"Administrator Menu – {currentUser.Username}", options);

            switch (choice)
            {
                case 1: _addProductView.Run();    break;
                case 2: _updateProductView.Run(); break;
                case 3: _deleteProductView.Run(); break;

                case 4: // Logout (always last)
                    ConsoleHelper.WriteInfo("Logged out successfully.");
                    ConsoleHelper.PressAnyKey();
                    return;
            }
        }
    }
}
