namespace ShopSmart.UI;

using ShopSmart.Services;

public class MainMenuView
{
    private readonly IUserService         _userService;
    private readonly UserRegistrationView _registrationView;

    // Future: IAuthService, customer/admin menu views added as constructor parameters.

    public MainMenuView(IUserService userService, UserRegistrationView registrationView)
    {
        _userService      = userService;
        _registrationView = registrationView;
    }

    /// <summary>Runs the main menu loop. Only exits when the user chooses Quit.</summary>
    public void Run()
    {
        while (true)
        {
            string[] options =
            [
                "Register",
                // Issue #2: "Login" will be inserted here
                "Quit"
            ];

            int choice = MenuRenderer.Show("ShopSmart - Main Menu", options);

            switch (choice)
            {
                case 1:
                    _registrationView.Run();
                    break;

                // case 2: _loginView.Run(); break;  <-- Issue #2 slot

                case 2:
                    ConsoleHelper.WriteInfo("Thank you for using ShopSmart. Goodbye!");
                    return;
            }
        }
    }
}
