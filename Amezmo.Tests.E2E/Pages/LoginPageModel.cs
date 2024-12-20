using System.Text.RegularExpressions;
using Amezmo.Tests.Library.Infrastructure.Interfaces;
using Microsoft.Playwright;

namespace Amezmo.Tests.Library.Infrastructure.PageModels;

public class LoginPageModel(IPage playwrightPage, IBrowserContext browserContext)
    : IPageModel
{
    public async Task GoToAsync()
    {
        IResponse? response = await playwrightPage.GotoAsync("/login");

        if (response is not { Status: 200 })
        {
            Assert.Fail("Unable to go to page: /login");
        }
    }

    public Task<string> GetPageBody()
    {
        return playwrightPage.ContentAsync();
    }

    public async Task ProvideUsernameAsync(string username)
    {
        ILocator input = playwrightPage.Locator("input[name=email]");
        await input.FocusAsync();
        
        await playwrightPage.Keyboard.InsertTextAsync(username);
    }

    public async Task ProvidePasswordAsync(string password)
    {
        ILocator input = playwrightPage.Locator("input[name=password]");
        await input.FocusAsync();
        await playwrightPage.Keyboard.InsertTextAsync(password);
    }

    public async Task<IResponse> PerformLoginAsync()
    {
        Func<Task> loginTask = async () =>
        {
            ILocator button = playwrightPage.Locator("role=button[name='Sign in']");
            await button.ClickAsync();
        };

        Regex matchLoginPage = new Regex(@"/login$");
        return await playwrightPage.RunAndWaitForResponseAsync(loginTask, matchLoginPage);
    }

    public async Task<bool> HasCookieAsync()
    {
        IReadOnlyList<BrowserContextCookiesResult> cookies = await browserContext.CookiesAsync();
        
        bool containsAuthCookieResult = cookies
            .Any(c => c is { Name: "az_presence", Domain: ".amezmo.com" });
        
        return containsAuthCookieResult;
    }

    public async Task<bool> IsLoggedInAsync()
    {
        ILocator profileMenu = playwrightPage.Locator("li[data-name='Profile Settings Menu']");
        await profileMenu.ClickAsync();
        
        ILocator logoutButton = playwrightPage.GetByText("Log out");
        await logoutButton.WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible,
        });
        
        return await logoutButton.IsVisibleAsync();
    }

    public async Task<bool> HasInvalidMessageAsync()
    {
        ILocator invalidMessageLocator = playwrightPage.GetByText("Invalid username or password");

        return await invalidMessageLocator.IsVisibleAsync();
    }

    public async Task<bool> IsDashboardUrlAsync()
    {
        string text = @"/sites/";
        return Regex.IsMatch(playwrightPage.Url, text, RegexOptions.IgnoreCase);
    }
}