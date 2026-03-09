namespace ShopSmart.UI;

using System.Text;

/// <summary>
/// Low-level console utilities: coloured output, prompts, and masked input.
/// </summary>
public static class ConsoleHelper
{
    // --- Output ---

    public static void WriteSuccess(string message) =>
        WriteColoured(message, ConsoleColor.Green);

    public static void WriteError(string message) =>
        WriteColoured(message, ConsoleColor.Red);

    public static void WriteWarning(string message) =>
        WriteColoured(message, ConsoleColor.Yellow);

    public static void WriteInfo(string message) =>
        WriteColoured(message, ConsoleColor.Cyan);

    // --- Inline variants (no newline) ---

    public static void WriteSuccessInline(string message) =>
        WriteColouredInline(message, ConsoleColor.Green);

    public static void WriteErrorInline(string message) =>
        WriteColouredInline(message, ConsoleColor.Red);

    public static void WriteWarningInline(string message) =>
        WriteColouredInline(message, ConsoleColor.Yellow);

    public static void WriteInfoInline(string message) =>
        WriteColouredInline(message, ConsoleColor.Cyan);

    private static void WriteColouredInline(string text, ConsoleColor colour)
    {
        ConsoleColor previous = Console.ForegroundColor;
        Console.ForegroundColor = colour;
        Console.Write(text);
        Console.ForegroundColor = previous;
    }

    public static void WriteHeading(string heading)
    {
        Console.WriteLine();
        WriteColoured(heading, ConsoleColor.White);
        WriteColoured(new string('-', heading.Length), ConsoleColor.DarkGray);
    }

    private static void WriteColoured(string text, ConsoleColor colour)
    {
        ConsoleColor previous = Console.ForegroundColor;
        Console.ForegroundColor = colour;
        Console.WriteLine(text);
        Console.ForegroundColor = previous;
    }

    // --- Input ---

    /// <summary>Prompts the user and returns a trimmed, non-null string.</summary>
    public static string Prompt(string label)
    {
        Console.Write($"  {label}: ");
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Prompts for a password, masking each character with '*'.
    /// Returns the raw (untrimmed) password string.
    /// </summary>
    public static string PromptPassword(string label = "Password")
    {
        Console.Write($"  {label}: ");
        var sb = new StringBuilder();

        while (true)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter)
                break;
            if (key.Key == ConsoleKey.Backspace && sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                sb.Append(key.KeyChar);
                Console.Write('*');
            }
        }

        Console.WriteLine();
        return sb.ToString();
    }

    /// <summary>Waits for the user to press any key before continuing.</summary>
    public static void PressAnyKey(string message = "Press any key to continue...")
    {
        WriteInfo(message);
        Console.ReadKey(intercept: true);
    }

    /// <summary>Clears the console and optionally prints a header.</summary>
    public static void ClearScreen(string? banner = null)
    {
        Console.Clear();
        if (banner is not null)
            WriteHeading(banner);
    }
}
