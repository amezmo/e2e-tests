using System.Diagnostics;
using System.Security.Cryptography;
using Amezmo.Tests.Library.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

namespace Amezmo.Tests.Library.Infrastructure.TestClasses;

public abstract class PageTest<T> : PlaywrightTest where T : class, IPageModel
{
    protected T Page { get; private set; }
    protected IBrowserContext _browserContext;
    protected TimeSpan TimeToCompleteFlow;

    public virtual Task RunAsync()
    {
        return Task.CompletedTask;
    }

    [OneTimeSetUp]
    public new async Task OneTimeSetup()
    {
        if (_browser is null)
        {
            throw new Exception("Could not instantiate the page because there is no browser configured.");
        }
        
        _browserContext = await _browser.NewContextAsync(new BrowserNewContextOptions()
        {
            BaseURL = Config["BASE_URL"],
        });
        
        IPage playwrightPage = await _browserContext.NewPageAsync();

        // if (typeof(T) != typeof(LoginPageModel))
        // {
        //     throw new Exception("Testing exception");
        // }

        T? pageModel = Activator.CreateInstance(typeof(T), [playwrightPage, _browserContext]) as T;

        Page = pageModel 
               ?? throw new Exception($"Could not instantiate the page model: [{pageModel?.GetType().FullName}].");
        
        // go to page and run flow
        await Page.GoToAsync();

        long startTimeStamp = Stopwatch.GetTimestamp();
        await RunAsync();
        TimeToCompleteFlow = Stopwatch.GetElapsedTime(startTimeStamp);
    }
}