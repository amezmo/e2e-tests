namespace Amezmo.Tests.Library.Infrastructure.Interfaces;

public interface ILoginPageModel : IPageModel
{
    /// <summary>
    /// Populates a username field
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public Task ProvideUsernameAsync(string username);
    
    /// <summary>
    /// Populates a password field
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public Task ProvidePasswordAsync(string password);
    
    /// <summary>
    /// Clicks the login button
    /// </summary>
    /// <returns></returns>
    public Task PerformLoginAsync(); // optionally return a new page model?
    
    public Task<bool> HasCookieAsync();
    public Task<bool> IsLoggedInAsync(); // check user in top right
}