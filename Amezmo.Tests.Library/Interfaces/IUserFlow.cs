namespace Amezmo.Tests.Library.Infrastructure.Interfaces;

/// <summary>
/// Represents a single user flow
/// </summary>
public interface IUserFlow
{
    Task RunAsync();
}