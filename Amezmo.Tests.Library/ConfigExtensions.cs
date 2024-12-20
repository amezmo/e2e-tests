using Microsoft.Extensions.Configuration;

namespace Amezmo.Tests.Library;

public static class ConfigExtensions
{
    public static void AddTestingEnvFile(this IConfigurationBuilder builder)
    {
        Dictionary<string, string?> settings = ReadEnvFile();
        builder.AddInMemoryCollection(settings);
    }
    
    private static Dictionary<string, string?> ReadEnvFile()
    {
        Dictionary<string, string?> envSettings = new();
        string currentDir = Directory.GetCurrentDirectory();

        do
        {
            currentDir = Path.Combine(currentDir, "..");
        } while (!Directory.GetFiles(currentDir, "*.sln").Any());
        
        string envFileLocation = Path.Combine(currentDir, "Amezmo.Tests.E2E", ".env");
        string envFileContents = File.ReadAllText(envFileLocation);
        
        foreach (var line in envFileContents.Split('\n'))
        {
            (string Name, string Value) setting = (line.Split("=")[0], line.Split("=")[1]);
            envSettings.Add(setting.Name, setting.Value);
        }
        
        return envSettings;
    }
}