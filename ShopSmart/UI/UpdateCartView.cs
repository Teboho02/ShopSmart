namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class UpdateCartView
{
    private readonly ICartService _cartService;

    public UpdateCartView(ICartService cartService) =>
        _cartService = cartService;

    /// <summary>
    /// Shows the cart, then prompts the user to select an item and set a new quantity.
    /// Quantity 0 removes the item. Loops until the user enters 0 to go back.
    /// </summary>
    public void Run(User currentUser)
    {
        while (true)
        {
            ConsoleHelper.ClearScreen("Update Cart");
            var items = _cartService.GetCart(currentUser);

            if (items.Count == 0)
            {
                ConsoleHelper.WriteWarning("Your cart is empty.");
                ConsoleHelper.PressAnyKey();
                return;
            }

            ViewCartView.RenderCartTable(items);
            Console.WriteLine();

            Console.Write("  Enter Product ID to update (or 0 to go back): ");
            string idInput = Console.ReadLine()?.Trim() ?? "0";

            if (idInput == "0" || idInput == string.Empty)
                return;

            if (!int.TryParse(idInput, out int productId))
            {
                ConsoleHelper.WriteError("Please enter a valid product ID.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            Console.Write("  New quantity (0 to remove): ");
            string qtyInput = Console.ReadLine()?.Trim() ?? "";

            if (!int.TryParse(qtyInput, out int newQty))
            {
                ConsoleHelper.WriteError("Please enter a valid quantity.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            try
            {
                _cartService.UpdateQuantity(currentUser, productId, newQty);
                Console.WriteLine();

                if (newQty == 0)
                    ConsoleHelper.WriteSuccess("Item removed from cart.");
                else
                    ConsoleHelper.WriteSuccess($"Quantity updated to {newQty}.");

                ConsoleHelper.PressAnyKey();
            }
            catch (ValidationException ex)
            {
                Console.WriteLine();
                ConsoleHelper.WriteError(ex.Message);
                ConsoleHelper.PressAnyKey();
            }
        }
    }
}
