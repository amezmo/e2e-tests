using Amezmo.Tests.Library.Infrastructure.Interfaces;
using Microsoft.Playwright;

namespace Amezmo.Tests.Library.Infrastructure.PageModels;

public class LoginPageModel(IPage playwrightPage, IBrowserContext browserContext) : ILoginPageModel
{
    public async Task GoToAsync()
    {
        IResponse? response = await playwrightPage.GotoAsync("/login");

        if (response is not { Status: 200 })
        {
            Assert.Fail("Unable to go to page: /login");
        }
    }

    public async Task ProvideUsernameAsync(string username)
    {
        ILocator input = playwrightPage.Locator("input[name=email]");
        await input.FocusAsync();
        await playwrightPage.Keyboard.TypeAsync(username);
    }

    public async Task ProvidePasswordAsync(string password)
    {
        ILocator input = playwrightPage.Locator("input[name=password]");
        await input.FocusAsync();
        await playwrightPage.Keyboard.TypeAsync(password);
    }

    public async Task PerformLoginAsync()
    {
        ILocator button = playwrightPage.Locator("role=button[name='Sign in']");
        await button.ClickAsync();
    }

    public async Task<bool> HasCookieAsync()
    {
        IReadOnlyList<BrowserContextCookiesResult> cookies = await browserContext.CookiesAsync();
        
        bool containsAuthCookieResult = cookies
            .Any(c => c.Name == "az_presence" && c.Domain == ".amezmo.com");
        
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
}