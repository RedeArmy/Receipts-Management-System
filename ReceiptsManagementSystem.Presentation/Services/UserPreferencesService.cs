using System.IO;
using System.Text.Json;

namespace ReceiptsManagementSystem.Presentation.Services;

public sealed class UserPreferencesService
{
    private readonly string _preferencesFilePath;
    private readonly UserPreferences _preferences;

    public UserPreferencesService()
    {
        var appDataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ReceiptsManagementSystem");

        Directory.CreateDirectory(appDataFolder);
        _preferencesFilePath = Path.Combine(appDataFolder, "user-preferences.json");

        _preferences = LoadPreferences();
    }

    public string PreferredLanguage
    {
        get => _preferences.Language;
        set
        {
            _preferences.Language = value;
            SavePreferences();
        }
    }

    private UserPreferences LoadPreferences()
    {
        if (!File.Exists(_preferencesFilePath))
        {
            return new UserPreferences { Language = "auto" };
        }

        try
        {
            var json = File.ReadAllText(_preferencesFilePath);
            return JsonSerializer.Deserialize<UserPreferences>(json)
                   ?? new UserPreferences { Language = "auto" };
        }
        catch
        {
            return new UserPreferences { Language = "auto" };
        }
    }

    private void SavePreferences()
    {
        try
        {
            var json = JsonSerializer.Serialize(_preferences, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_preferencesFilePath, json);
        }
        catch
        {
            // Fail silently - preferences are not critical
        }
    }
}

public sealed class UserPreferences
{
    public string Language { get; set; } = "auto";
}
