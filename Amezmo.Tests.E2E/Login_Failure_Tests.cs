using Amezmo.Tests.Library.Infrastructure.PageModels;
using Amezmo.Tests.Library.Infrastructure.TestClasses;
using FluentAssertions;
using Microsoft.Playwright;

namespace Amezmo.Tests.E2E;

public class Login_Failure_Tests : PageTest<LoginPageModel>
{
    private IResponse? _loginResponse;
    
    public override async Task RunAsync()
    {
        await Page.ProvideUsernameAsync("not_a_valid_username@email.com");
        await Page.ProvidePasswordAsync("not_a_valid_password");
        _loginResponse = await Page.PerformLoginAsync();
    }

    [Test]
    [Ignore("Cookie will exist regardless of successful/unsuccessful login attempt")]
    public async Task Should_Not_Receive_Cookie()
    {
        bool result = await Page.HasCookieAsync();

        result
            .Should()
            .BeFalse();
    }
    
    [Test]
    public async Task Should_Receive_Invalid_Form_Message()
    {
        bool result = await Page.HasInvalidMessageAsync();

        result
            .Should()
            .BeTrue();
    }
    
    [Test]
    public void Should_Have_Correct_Http_Response()
    {
        Assert.IsNotNull(_loginResponse, "Login response was not received");
        
        bool getHeaderResult = _loginResponse.Headers.TryGetValue("location", out string? location) && 
                               !string.IsNullOrWhiteSpace(location);

        getHeaderResult
            .Should()
            .BeTrue();

        location
            .Should()
            .NotBeNullOrWhiteSpace();
        
        location
            .Should()
            .EndWith("/login");
    }
}