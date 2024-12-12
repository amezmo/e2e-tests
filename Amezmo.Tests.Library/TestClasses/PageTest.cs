using Amezmo.Tests.Library.Infrastructure.Interfaces;
using Amezmo.Tests.Library.Infrastructure.PageModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

namespace Amezmo.Tests.Library.Infrastructure.TestClasses;

public class PageTest<T>(IConfiguration config) 
    : PlaywrightTest(config) where T : IPageModel
{
    protected T Page { get; private set; }
    protected IBrowserContext _browserContext;
    private readonly IConfiguration _config = config;

    [OneTimeSetUp]
    public new async Task OneTimeSetup()
    {
        if (_browser is null)
        {
            throw new Exception("Could not instantiate the page because there is no browser configured.");
        }
        
        _browserContext = await _browser.NewContextAsync(new BrowserNewContextOptions()
        {
            BaseURL = _config["baseUrl"] ?? "",
        });
        
        IPage playwrightPage = await _browserContext.NewPageAsync();

        if (typeof(T) != typeof(LoginPageModel))
        {
            throw new Exception("Testing exception");
        }
        
        Page = (T)(object)(new LoginPageModel(playwrightPage, _browserContext));
        
        
        await Page.GoToAsync();
    }
}