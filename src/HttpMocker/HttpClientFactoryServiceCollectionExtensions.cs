using Microsoft.Extensions.DependencyInjection;

namespace HttpMocker;

/// <summary>
/// Extension methods for registering HTTP client mocks into an <see cref="IServiceCollection" />.
/// </summary>
public static class HttpClientFactoryServiceCollectionExtensions
{
    /// <summary>
    /// Register HTTP client mock for a type
    /// </summary>
    /// <typeparam name="TClient">A HttpClient type to mock</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>An <see cref="IHttpClientMockBuilder"/> for creating and configuring the HTTP client mock.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="services"/> is <c>null</c>.</exception>
    public static IHttpClientMockBuilder AddHttpClientMock<TClient>(this IServiceCollection services) where TClient : class
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var name = TypeNameHelper.TypeNameHelper.GetTypeDisplayName(typeof(TClient), fullName: false);
        
        return AddHttpClientMockCore(services, name);
    }


    /// <summary>
    /// Register HTTP client mock for a client name
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="clientName">A HttpClient name to mock.</param>
    /// <returns>An <see cref="IHttpClientMockBuilder"/> for creating and configuring the HTTP client mock.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="services"/> is <c>null</c>.</exception>
    public static IHttpClientMockBuilder AddHttpClientMock(this IServiceCollection services, string clientName)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        return AddHttpClientMockCore(services, clientName);
    }

    /// <summary>
    /// Register HTTP client mock for all registered HttpClient services
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>An <see cref="IHttpClientMockBuilder"/> for creating and configuring the HTTP client mock.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="services"/> is <c>null</c>.</exception>
    public static IHttpClientMockBuilder AddGlobalHttpClientMock(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var builder = new HttpClientMockBuilder(services, "global-http-client");
        builder.AddGlobalDelegatingHandlerFake();

        return builder;
    }
    
    private static IHttpClientMockBuilder AddHttpClientMockCore(IServiceCollection services, string name)
    {
        var builder = new HttpClientMockBuilder(services, name);
        builder.AddDelegatingHandlerFake();

        return builder;
    }
}