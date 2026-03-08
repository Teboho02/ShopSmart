namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class AddWalletFundsView
{
    private readonly IUserService _userService;

    public AddWalletFundsView(IUserService userService) =>
        _userService = userService;

    /// <summary>Prompts for a deposit amount, validates it, credits the wallet, and persists.</summary>
    public void Run(User currentUser)
    {
        ConsoleHelper.ClearScreen("Add Wallet Funds");
        Console.WriteLine();
        ConsoleHelper.WriteInfo($"  Current balance: {currentUser.WalletBalance:C}");
        Console.WriteLine();
        Console.Write("  Enter amount to add: $");
        string input = Console.ReadLine()?.Trim() ?? "";

        if (!decimal.TryParse(input, out decimal amount))
        {
            ConsoleHelper.WriteError("Please enter a valid amount.");
            ConsoleHelper.PressAnyKey();
            return;
        }

        try
        {
            _userService.TopUpWallet(currentUser, amount);
            Console.WriteLine();
            ConsoleHelper.WriteSuccess($"  Added {amount:C}. New balance: {currentUser.WalletBalance:C}");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine();
            ConsoleHelper.WriteError(ex.Message);
        }

        ConsoleHelper.PressAnyKey();
    }
}
