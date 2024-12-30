using Amezmo.Tests.Library.Infrastructure.Interfaces;
using Amezmo.Tests.Library.Infrastructure.PageModels;
using Amezmo.Tests.Library.Infrastructure.TestClasses;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

namespace Amezmo.Tests.E2E;

public class Login_Success_Tests : PageTest<LoginPageModel>
{
    private IResponse _loginResponse = null!;
    private IConfigurationSection credentials => Config.GetSection("Credentials");
    
    public override async Task RunAsync()
    {
        IConfigurationSection credentials = Config.GetSection("Credentials");
        
        await Page.ProvideUsernameAsync(credentials["username"] ?? "");
        await Page.ProvidePasswordAsync(credentials["password"] ?? "");
        _loginResponse = await Page.PerformLoginAsync();
    }

    // [Test]
    // public void Login_Flow_Completed_Under_2_Seconds()
    // {
    //     StopWatch.Elapsed
    //         .Should()
    //         .BeLessThan(TimeSpan.FromSeconds(2));
    // }

    [Test]
    public void Should_Have_Provided_Credentials()
    {
        credentials
            .Should()
            .NotBeNull("Credentials are empty");
    }
    
    [Test]
    public void Should_Have_Provided_Username()
    {
        credentials["username"]
            .Should()
            .NotBeNullOrWhiteSpace("Username is empty");
    }
    
    [Test]
    public void Should_Have_Provided_Password()
    {
        credentials["password"]
            .Should()
            .NotBeNullOrWhiteSpace("Password is empty");
    }
    
    [Test]
    [Ignore("Cookie will exist regardless of successful/unsuccessful login attempt")]
    public async Task Should_Have_Valid_Cookie()
    {
        bool result = await Page.HasCookieAsync();

        result
            .Should()
            .BeTrue();
    }
    
    [Test]
    public async Task Should_Be_Logged_In()
    {
        bool result = await Page.IsLoggedInAsync();

        result
            .Should()
            .BeTrue();
    }
    
    [Test]
    public void Should_Have_Response_Location_Header_Changed()
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
            .NotEndWith("/login");
    }
    
    [Test]
    public async Task Should_Have_Redirected_Client_To_Dashboard()
    {
        bool result = await Page.IsDashboardUrlAsync();

        result
            .Should()
            .BeTrue();
    }
}
