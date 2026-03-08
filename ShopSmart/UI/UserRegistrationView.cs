namespace ShopSmart.UI;

using ShopSmart.Enums;
using ShopSmart.Services;

public class UserRegistrationView
{
    private readonly IUserService _userService;

    public UserRegistrationView(IUserService userService) =>
        _userService = userService;

    /// <summary>
    /// Runs the registration form. Loops on validation failure so the user
    /// can correct mistakes without being sent back to the main menu.
    /// </summary>
    public void Run()
    {
        ConsoleHelper.ClearScreen("User Registration");

        while (true)
        {
            string   username = ConsoleHelper.Prompt("Username");
            string   email    = ConsoleHelper.Prompt("Email");
            string   password = ConsoleHelper.PromptPassword("Password (min 6 chars)");
            UserRole role     = PromptRole();

            try
            {
                var user = _userService.Register(username, email, password, role);
                Console.WriteLine();
                ConsoleHelper.WriteSuccess($"Registration successful! Welcome, {user.Username}.");
                ConsoleHelper.PressAnyKey();
                return;
            }
            catch (ValidationException ex)
            {
                Console.WriteLine();
                ConsoleHelper.WriteError($"Registration failed: {ex.Message}");
                Console.WriteLine();
                ConsoleHelper.WriteWarning("Please try again.");
                Console.WriteLine();
            }
        }
    }

    private static UserRole PromptRole()
    {
        int choice = MenuRenderer.Show("Select Account Type", ["Customer", "Administrator"]);
        return choice == 2 ? UserRole.Administrator : UserRole.Customer;
    }
}
