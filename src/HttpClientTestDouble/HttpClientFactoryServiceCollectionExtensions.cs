using Microsoft.Extensions.DependencyInjection;

namespace HttpClientTestDouble;

public static class HttpClientFactoryServiceCollectionExtensions
{
    public static IHttpClientFakeBuilder AddHttpClientFake<TClient>(this IServiceCollection services) where TClient : class
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var name = TypeNameHelper.TypeNameHelper.GetTypeDisplayName(typeof(TClient), fullName: false);
        
        var builder = new HttpClientFakeBuilder(services, name);
        builder.AddDelegatingHandlerFake();

        return builder;
    }
    
    public static IHttpClientFakeBuilder AddGlobalHttpClientFake(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var builder = new HttpClientFakeBuilder(services, "global-http-client");
        builder.AddGlobalDelegatingHandlerFake();

        return builder;
    }
}