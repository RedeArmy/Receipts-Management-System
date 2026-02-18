using System.IO;

namespace ReceiptsManagementSystem.Presentation.Configuration;

public sealed class DatabaseSettings
{
    public const string SectionName = "Database";

    private string ConnectionString { get; init; } = string.Empty;
    
    public string ResolvedConnectionString
    {
        get
        {
            var appDir = AppContext.BaseDirectory;

            // Asegura que el directorio termina con separador
            if (!appDir.EndsWith(Path.DirectorySeparatorChar))
            {
                appDir += Path.DirectorySeparatorChar;
            }

            return ConnectionString.Replace("{AppDir}", appDir);
        }
    }
}