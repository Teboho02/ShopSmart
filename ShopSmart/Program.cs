Console.WindowHeight = 10000;

// --- Composition Root ---
// All concrete types are instantiated here. Every other class receives its
// dependencies through constructor parameters — no `new` calls outside this file.

var appData           = new ShopSmart.Data.AppData();
var userRepository    = new ShopSmart.Data.UserRepository(appData);
var productRepository = new ShopSmart.Data.ProductRepository(appData);
var userService       = new ShopSmart.Services.UserService(userRepository);
var productService    = new ShopSmart.Services.ProductService(productRepository);
var registrationView  = new ShopSmart.UI.UserRegistrationView(userService);
var loginView         = new ShopSmart.UI.UserLoginView(userService);
var browseView        = new ShopSmart.UI.BrowseProductsView(productService);
var customerMenuView  = new ShopSmart.UI.CustomerMenuView(productService, browseView);
var adminMenuView     = new ShopSmart.UI.AdminMenuView();
var mainMenu          = new ShopSmart.UI.MainMenuView(userService, registrationView, loginView, customerMenuView, adminMenuView);

if (productRepository.GetAll().Count == 0)
    SeedProducts(productRepository);

mainMenu.Run();

// --- Seed Data ---
static void SeedProducts(ShopSmart.Data.ProductRepository repo)
{
    repo.Add(new ShopSmart.Models.Product(repo.NextProductId(), "Wireless Headphones",  "Over-ear noise-cancelling Bluetooth headphones.", "Electronics", 49.99m,  15));
    repo.Add(new ShopSmart.Models.Product(repo.NextProductId(), "USB-C Hub",            "7-in-1 USB-C hub with HDMI and SD card reader.",  "Electronics", 29.99m,   0));
    repo.Add(new ShopSmart.Models.Product(repo.NextProductId(), "Mechanical Keyboard",  "TKL mechanical keyboard with blue switches.",     "Electronics", 89.99m,   8));
    repo.Add(new ShopSmart.Models.Product(repo.NextProductId(), "Cotton T-Shirt",       "100% cotton crew-neck t-shirt.",                  "Clothing",    14.99m,  50));
    repo.Add(new ShopSmart.Models.Product(repo.NextProductId(), "Running Shoes",        "Lightweight mesh running shoes.",                 "Clothing",    59.99m,  20));
    repo.Add(new ShopSmart.Models.Product(repo.NextProductId(), "Stainless Water Bottle","500ml insulated stainless steel water bottle.",  "Kitchen",     19.99m,  35));
}
