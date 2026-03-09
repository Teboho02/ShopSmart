namespace ShopSmart.Data;

using System.Text.Json;

/// <summary>
/// Generic JSON file persistence utility.
/// Handles directory creation, serialization, and deserialization.
/// </summary>
public static class JsonFileStore
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    /// <summary>
    /// Loads a list of items from a JSON file.
    /// Returns an empty list if the file does not exist.
    /// </summary>
    public static List<T> Load<T>(string filePath)
    {
        if (!File.Exists(filePath))
            return [];

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<T>>(json, Options) ?? [];
    }

    /// <summary>
    /// Saves a collection of items to a JSON file, creating the directory if needed.
    /// </summary>
    public static void Save<T>(string filePath, IEnumerable<T> items)
    {
        string? dir = Path.GetDirectoryName(filePath);
        if (dir is not null)
            Directory.CreateDirectory(dir);

        File.WriteAllText(filePath, JsonSerializer.Serialize(items, Options));
    }
}
