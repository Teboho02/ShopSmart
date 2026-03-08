namespace ShopSmart.UI;

using ShopSmart.Models;

public class AdminMenuView
{
    // Future: IProductService injected here as a constructor parameter
    // when Issues #14–#18 are implemented.

    /// <summary>Runs the administrator menu loop. Returns when the user logs out.</summary>
    public void Run(User currentUser)
    {
        while (true)
        {
            string[] options =
            [
                // Issues #14–#18 options will be inserted above Logout.
                "Logout"
            ];

            int choice = MenuRenderer.Show($"Administrator Menu – {currentUser.Username}", options);

            switch (choice)
            {
                case 1: // Logout (last option — always keep this last)
                    ConsoleHelper.WriteInfo("Logged out successfully.");
                    ConsoleHelper.PressAnyKey();
                    return;
            }
        }
    }
}
