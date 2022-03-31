using Microsoft.Extensions.DependencyInjection;

namespace HttpMocker;

/// <summary>
/// Defines a class that provides the mechanisms to configure an HTTP client mock.
/// </summary>
public interface IHttpClientMockBuilder
{
    /// <summary>
    /// Builder name that is used to distinguish between multiple HTTP client mocks
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The <see cref="IServiceCollection"/> for adding services.
    /// </summary>
    IServiceCollection Services { get; }
}