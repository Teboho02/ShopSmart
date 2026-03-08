namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class UserLoginView
{
    private readonly IUserService _userService;

    public UserLoginView(IUserService userService) =>
        _userService = userService;

    /// <summary>
    /// Runs the login form. Loops on failure so the user can retry without
    /// returning to the main menu. Returns the authenticated User on success.
    /// </summary>
    public User Run()
    {
        ConsoleHelper.ClearScreen("Login");

        while (true)
        {
            string username = ConsoleHelper.Prompt("Username");
            string password = ConsoleHelper.PromptPassword();

            try
            {
                var user = _userService.Login(username, password);
                Console.WriteLine();
                ConsoleHelper.WriteSuccess($"Welcome back, {user.Username}! ({user.Role})");
                ConsoleHelper.PressAnyKey();
                return user;
            }
            catch (ValidationException ex)
            {
                Console.WriteLine();
                ConsoleHelper.WriteError(ex.Message);
                Console.WriteLine();
                ConsoleHelper.WriteWarning("Please try again.");
                Console.WriteLine();
            }
        }
    }
}
