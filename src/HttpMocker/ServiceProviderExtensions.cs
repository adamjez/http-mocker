using HttpMocker.Middlewares;
using Microsoft.Extensions.DependencyInjection;

namespace HttpMocker;

/// <summary>
/// Extension methods for getting captured requests from an <see cref="IServiceProvider" />.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Get captured requests.
    /// </summary>
    /// <param name="provider">The <see cref="IServiceProvider"/> to retrieve the service object from.</param>
    /// <returns>A collection of captured HTTP requests.</returns>
    public static IEnumerable<HttpRequestMessage> GetCapturedRequests(this IServiceProvider provider)
    {
        return provider.GetRequiredService<RequestCapturingMiddleware>().Requests;
    }
}
