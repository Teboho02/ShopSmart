namespace ShopSmart.UI;

using ShopSmart.Models;

public class CustomerMenuView
{
    // Future: IProductService, ICartService, IOrderService, IReviewService
    // injected here as constructor parameters when each issue is implemented.

    /// <summary>Runs the customer menu loop. Returns when the user logs out.</summary>
    public void Run(User currentUser)
    {
        while (true)
        {
            string[] options =
            [
                // Issues #3–#13 options will be inserted above Logout.
                "Logout"
            ];

            int choice = MenuRenderer.Show($"Customer Menu – {currentUser.Username}", options);

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
