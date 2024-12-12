using Amezmo.Tests.Library.Infrastructure.Interfaces;
using Amezmo.Tests.Library.Infrastructure.PageModels;
using Microsoft.Extensions.Configuration;

namespace Amezmo.Tests.Library.Infrastructure.Flows;

public class LoginFlow(LoginPageModel page, IConfiguration config) : IUserFlow
{
    private LoginPageModel _page { get; } = page;
    
    private (string Username, string Password) _credentials => 
        new(config["username"] ?? "", config["password"] ?? "");

    public async Task RunAsync()
    {
        await _page.ProvideUsernameAsync(_credentials.Username);
        await _page.ProvidePasswordAsync(_credentials.Password);
        await _page.PerformLoginAsync();
    }
}