// --- Composition Root ---
// All concrete types are instantiated here. Every other class receives its
// dependencies through constructor parameters — no `new` calls outside this file.

var appData           = new ShopSmart.Data.AppData();
var userRepository    = new ShopSmart.Data.UserRepository(appData);
var productRepository = new ShopSmart.Data.ProductRepository(appData);
var orderRepository   = new ShopSmart.Data.OrderRepository(appData);
var paymentRepository = new ShopSmart.Data.PaymentRepository(appData);
var cartRepository    = new ShopSmart.Data.CartRepository(appData);
var userService       = new ShopSmart.Services.UserService(userRepository);
var productService    = new ShopSmart.Services.ProductService(productRepository);
var cartService       = new ShopSmart.Services.CartService(cartRepository, productService);
var orderService      = new ShopSmart.Services.OrderService(
                            cartRepository, productService, productRepository,
                            orderRepository, paymentRepository, userRepository);
var registrationView  = new ShopSmart.UI.UserRegistrationView(userService);
var loginView         = new ShopSmart.UI.UserLoginView(userService);
var browseView        = new ShopSmart.UI.BrowseProductsView(productService);
var searchView        = new ShopSmart.UI.SearchProductsView(productService);
var addToCartView     = new ShopSmart.UI.AddToCartView(productService, cartService);
var viewCartView      = new ShopSmart.UI.ViewCartView(cartService);
var updateCartView    = new ShopSmart.UI.UpdateCartView(cartService);
var checkoutView      = new ShopSmart.UI.CheckoutView(cartService, orderService);
var reviewRepository   = new ShopSmart.Data.ReviewRepository(appData);
var reviewService      = new ShopSmart.Services.ReviewService(orderRepository, reviewRepository);
var walletBalanceView  = new ShopSmart.UI.WalletBalanceView();
var addWalletFundsView = new ShopSmart.UI.AddWalletFundsView(userService);
var orderHistoryView   = new ShopSmart.UI.OrderHistoryView(orderService);
var trackOrderView     = new ShopSmart.UI.TrackOrderView(orderService);
var reviewProductsView = new ShopSmart.UI.ReviewProductsView(reviewService);
var customerMenuView   = new ShopSmart.UI.CustomerMenuView(
                             productService, browseView, searchView,
                             cartService, addToCartView,
                             viewCartView, updateCartView, checkoutView,
                             walletBalanceView, addWalletFundsView, orderHistoryView,
                             trackOrderView, reviewProductsView);
var addProductView         = new ShopSmart.UI.AddProductView(productService);
var updateProductView      = new ShopSmart.UI.UpdateProductView(productService);
var deleteProductView      = new ShopSmart.UI.DeleteProductView(productService);
var restockProductView     = new ShopSmart.UI.RestockProductView(productService);
var viewAllProductsView    = new ShopSmart.UI.ViewAllProductsView(productService);
var updateOrderStatusView  = new ShopSmart.UI.UpdateOrderStatusView(orderService, userService);
var adminMenuView          = new ShopSmart.UI.AdminMenuView(productService, addProductView,
                                                            updateProductView, deleteProductView,
                                                            restockProductView, viewAllProductsView,
                                                            updateOrderStatusView);
var mainMenu          = new ShopSmart.UI.MainMenuView(
                            userService, registrationView, loginView,
                            customerMenuView, adminMenuView);


mainMenu.Run();

