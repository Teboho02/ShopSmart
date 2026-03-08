namespace ShopSmart.UI;

using ShopSmart.Models;
using ShopSmart.Services;

public class ReviewProductsView
{
    private readonly IReviewService _reviewService;

    public ReviewProductsView(IReviewService reviewService) =>
        _reviewService = reviewService;

    /// <summary>
    /// Lists products from delivered orders that the user hasn't reviewed yet,
    /// then collects a star rating and optional comment. Loops until back.
    /// </summary>
    public void Run(User currentUser)
    {
        while (true)
        {
            ConsoleHelper.ClearScreen("Review Products");
            var reviewable = _reviewService.GetReviewableProducts(currentUser);

            if (reviewable.Count == 0)
            {
                Console.WriteLine();
                ConsoleHelper.WriteWarning("No products available to review. Complete a delivery first.");
                ConsoleHelper.PressAnyKey();
                return;
            }

            Console.WriteLine();
            ConsoleHelper.WriteHeading("Products you can review:");
            for (int i = 0; i < reviewable.Count; i++)
                Console.WriteLine($"  {i + 1}. {reviewable[i].ProductName}");

            Console.WriteLine();
            Console.Write("  Select product to review (or 0 to go back): ");
            string indexInput = Console.ReadLine()?.Trim() ?? "0";

            if (indexInput == "0" || indexInput == string.Empty)
                return;

            if (!int.TryParse(indexInput, out int selection) ||
                selection < 1 || selection > reviewable.Count)
            {
                ConsoleHelper.WriteError("Please enter a number from the list.");
                ConsoleHelper.PressAnyKey();
                continue;
            }

            var (productId, productName) = reviewable[selection - 1];

            // Collect rating
            int rating;
            while (true)
            {
                Console.Write("  Rating [1-5]: ");
                string ratingInput = Console.ReadLine()?.Trim() ?? "";
                if (int.TryParse(ratingInput, out rating) && rating >= 1 && rating <= 5)
                    break;
                ConsoleHelper.WriteError("Please enter a number between 1 and 5.");
            }

            Console.Write("  Comment (optional, press Enter to skip): ");
            string comment = Console.ReadLine()?.Trim() ?? "";

            try
            {
                _reviewService.SubmitReview(currentUser, productId, productName, rating, comment);
                Console.WriteLine();
                ConsoleHelper.WriteSuccess("Review submitted!");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine();
                ConsoleHelper.WriteError(ex.Message);
            }

            ConsoleHelper.PressAnyKey();
        }
    }
}
