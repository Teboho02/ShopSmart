namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class AdminMenuView
{
    private readonly IProductService    _productService;
    private readonly AddProductView     _addProductView;
    private readonly UpdateProductView  _updateProductView;
    private readonly DeleteProductView  _deleteProductView;
    private readonly RestockProductView _restockProductView;
    private readonly ViewAllProductsView _viewAllProductsView;

    public AdminMenuView(
        IProductService     productService,
        AddProductView      addProductView,
        UpdateProductView   updateProductView,
        DeleteProductView   deleteProductView,
        RestockProductView  restockProductView,
        ViewAllProductsView viewAllProductsView)
    {
        _productService      = productService;
        _addProductView      = addProductView;
        _updateProductView   = updateProductView;
        _deleteProductView   = deleteProductView;
        _restockProductView  = restockProductView;
        _viewAllProductsView = viewAllProductsView;
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
                "Restock Product",
                "View All Products",
                "Logout"
            ];

            int choice = MenuRenderer.Show($"Administrator Menu – {currentUser.Username}", options);

            switch (choice)
            {
                case 1: _addProductView.Run();      break;
                case 2: _updateProductView.Run();   break;
                case 3: _deleteProductView.Run();   break;
                case 4: _restockProductView.Run();  break;
                case 5: _viewAllProductsView.Run(); break;

                case 6: // Logout (always last)
                    ConsoleHelper.WriteInfo("Logged out successfully.");
                    ConsoleHelper.PressAnyKey();
                    return;
            }
        }
    }
}
