namespace ShopSmart.UI;

using ShopSmart.Models;

public class WalletBalanceView
{
    /// <summary>Displays the user's current wallet balance then waits for a keypress.</summary>
    public void Run(User currentUser)
    {
        ConsoleHelper.ClearScreen("Wallet Balance");
        Console.WriteLine();
        ConsoleHelper.WriteInfo($"  Current Wallet Balance: {currentUser.WalletBalance:C}");
        ConsoleHelper.PressAnyKey();
    }
}
