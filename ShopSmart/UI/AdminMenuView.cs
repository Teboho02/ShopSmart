namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class AdminMenuView
{
    private readonly IProductService       _productService;
    private readonly AddProductView        _addProductView;
    private readonly UpdateProductView     _updateProductView;
    private readonly DeleteProductView     _deleteProductView;
    private readonly RestockProductView    _restockProductView;
    private readonly ViewAllProductsView   _viewAllProductsView;
    private readonly UpdateOrderStatusView _updateOrderStatusView;
    private readonly LowStockView          _lowStockView;

    public AdminMenuView(
        IProductService        productService,
        AddProductView         addProductView,
        UpdateProductView      updateProductView,
        DeleteProductView      deleteProductView,
        RestockProductView     restockProductView,
        ViewAllProductsView    viewAllProductsView,
        UpdateOrderStatusView  updateOrderStatusView,
        LowStockView           lowStockView)
    {
        _productService        = productService;
        _addProductView        = addProductView;
        _updateProductView     = updateProductView;
        _deleteProductView     = deleteProductView;
        _restockProductView    = restockProductView;
        _viewAllProductsView   = viewAllProductsView;
        _updateOrderStatusView = updateOrderStatusView;
        _lowStockView          = lowStockView;
    }

    /// <summary>Runs the administrator menu loop. Returns when the user logs out.</summary>
    public void Run(User currentUser)
    {
        while (true)
        {
            var lowCount  = _productService.GetLowStock().Count;
            var badge     = lowCount > 0 ? $"  ⚠ {lowCount} low stock" : string.Empty;
            var menuTitle = $"Administrator Menu – {currentUser.Username}{badge}";

            string[] options =
            [
                "Add Product",
                "Update Product",
                "Delete Product",
                "Restock Product",
                "View All Products",
                "Update Order Status",
                "Low Stock Report",
                "Logout"
            ];

            int choice = MenuRenderer.Show(menuTitle, options);

            switch (choice)
            {
                case 1: _addProductView.Run();        break;
                case 2: _updateProductView.Run();     break;
                case 3: _deleteProductView.Run();     break;
                case 4: _restockProductView.Run();    break;
                case 5: _viewAllProductsView.Run();   break;
                case 6: _updateOrderStatusView.Run(); break;
                case 7: _lowStockView.Run();          break;

                case 8: // Logout (always last)
                    ConsoleHelper.WriteInfo("Logged out successfully.");
                    ConsoleHelper.PressAnyKey();
                    return;
            }
        }
    }
}
