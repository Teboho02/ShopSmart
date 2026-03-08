Console.WindowHeight = 10000;

// --- Composition Root ---
// All concrete types are instantiated here. Every other class receives its
// dependencies through constructor parameters — no `new` calls outside this file.

var appData          = new ShopSmart.Data.AppData();
var userRepository   = new ShopSmart.Data.UserRepository(appData);
var userService      = new ShopSmart.Services.UserService(userRepository);
var registrationView = new ShopSmart.UI.UserRegistrationView(userService);
var mainMenu         = new ShopSmart.UI.MainMenuView(userService, registrationView);

mainMenu.Run();
