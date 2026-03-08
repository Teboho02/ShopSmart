namespace ShopSmart.UI;

/// <summary>
/// Renders a numbered list of menu options and reads the user's selection.
/// Reusable for every menu in the application.
/// </summary>
public static class MenuRenderer
{
    /// <summary>
    /// Displays the menu and returns the 1-based index of the chosen option.
    /// Loops until valid input is received.
    /// </summary>
    public static int Show(string title, IReadOnlyList<string> options)
    {
        while (true)
        {
            ConsoleHelper.ClearScreen(title);
            Console.WriteLine();

            for (int i = 0; i < options.Count; i++)
                Console.WriteLine($"  [{i + 1}] {options[i]}");

            Console.WriteLine();
            string input = ConsoleHelper.Prompt("Enter choice");

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= options.Count)
                return choice;

            ConsoleHelper.WriteError($"Please enter a number between 1 and {options.Count}.");
            ConsoleHelper.PressAnyKey();
        }
    }
}
