using System.IO;

namespace ReceiptsManagementSystem.Presentation.Configuration;

public sealed class DatabaseSettings
{
    public const string SectionName = "Database";

    public string ConnectionString { get; init; } = string.Empty;

    public string ResolvedConnectionString
    {
        get
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                throw new InvalidOperationException("ConnectionString is not configured");
            }

            var appDir = AppContext.BaseDirectory;

            if (!appDir.EndsWith(Path.DirectorySeparatorChar))
            {
                appDir += Path.DirectorySeparatorChar;
            }

            var resolved = ConnectionString.Replace("{AppDir}", appDir);

            return resolved;
        }
    }
}
