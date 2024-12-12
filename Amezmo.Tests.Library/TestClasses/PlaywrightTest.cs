using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

namespace Amezmo.Tests.Library.Infrastructure.TestClasses;

[TestFixture]
public abstract class PlaywrightTest(IConfiguration config)
{
    protected IPlaywright _playwright;
    protected IBrowser? _browser;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        // install playwright (through dotnet)
        int exitCode = Microsoft.Playwright.Program.Main(["install"]);

        if (exitCode != 0)
        {
            throw new Exception($"Failed to install Playwright browsers: {exitCode}");
        }
        
        // Initialize Playwright and launch the browser once for all tests
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = bool.Parse(config["headless"] ?? "false"),
            SlowMo = TimeSpan.FromSeconds(3).Milliseconds,
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