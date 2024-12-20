using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

namespace Amezmo.Tests.Library.Infrastructure.TestClasses;

[TestFixture]
public abstract class PlaywrightTest
{
    protected IPlaywright _playwright;
    protected IBrowser? _browser;
    
    protected IConfiguration Config { get; private set; }

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        IConfigurationBuilder configBuilder = new ConfigurationBuilder()
            .AddEnvironmentVariables();
        
        if (Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == "Development")
        {
            configBuilder.AddTestingEnvFile();
            configBuilder.AddUserSecrets("001165e4-a2a1-4b4b-869e-a450a8683ac5");
            
            // install playwright (through dotnet)
            int exitCode = Microsoft.Playwright.Program.Main(["install"]);

            if (exitCode != 0)
            {
                throw new Exception($"Failed to install Playwright browsers: {exitCode}");
            }
        }
        
        Config = configBuilder.Build();
        
        // Initialize Playwright and launch the browser once for all tests
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = bool.Parse(Config["HEADLESS"] ?? "true"),
            SlowMo = int.Parse(Config["SLOWMO"] ?? "0"),
        });
    }

    [OneTimeTearDown]
    public async Task OneTimeTeardown()
    {
        // Close the browser after all tests are done
        if (_browser is not null)
        {
            await _browser.CloseAsync();
        }
        _playwright?.Dispose();
    }

    // [Test]
    // public async Task VerifyPageTitle()
    // {
    //     // Use the same browser for all tests
    //     var page = await _browser.NewPageAsync();
    //     await page.GotoAsync("https://example.com");
    //     var title = await page.TitleAsync();
    //     Assert.AreEqual("Example Domain", title);
    // }
    //
    // [Test]
    // public async Task VerifyPageContent()
    // {
    //     // Same browser instance used for this test
    //     var page = await _browser.NewPageAsync();
    //     await page.GotoAsync("https://example.com");
    //     var content = await page.TextContentAsync("h1");
    //     Assert.AreEqual("Example Domain", content);
    // }
}