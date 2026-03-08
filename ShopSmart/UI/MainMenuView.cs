namespace ShopSmart.UI;

using ShopSmart.Enums;
using ShopSmart.Services;

public class MainMenuView
{
    private readonly IUserService         _userService;
    private readonly UserRegistrationView _registrationView;
    private readonly UserLoginView        _loginView;
    private readonly CustomerMenuView     _customerMenuView;
    private readonly AdminMenuView        _adminMenuView;

    public MainMenuView(
        IUserService         userService,
        UserRegistrationView registrationView,
        UserLoginView        loginView,
        CustomerMenuView     customerMenuView,
        AdminMenuView        adminMenuView)
    {
        _userService      = userService;
        _registrationView = registrationView;
        _loginView        = loginView;
        _customerMenuView = customerMenuView;
        _adminMenuView    = adminMenuView;
    }

    /// <summary>Runs the main menu loop. Only exits when the user chooses Quit.</summary>
    public void Run()
    {
        while (true)
        {
            string[] options =
            [
                "Register",
                "Login",
                "Quit"
            ];

            int choice = MenuRenderer.Show("ShopSmart - Main Menu", options);

            switch (choice)
            {
                case 1:
                    _registrationView.Run();
                    break;

                case 2:
                    var user = _loginView.Run();
                    if (user.Role == UserRole.Administrator)
                        _adminMenuView.Run(user);
                    else
                        _customerMenuView.Run(user);
                    break;

                case 3:
                    ConsoleHelper.WriteInfo("Thank you for using ShopSmart. Goodbye!");
                    return;
            }
        }
    }
}
