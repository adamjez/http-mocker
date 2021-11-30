using Microsoft.Extensions.DependencyInjection;

namespace HttpMocker;

public static class HttpClientFactoryServiceCollectionExtensions
{
    public static IHttpClientMockBuilder AddHttpClientMock<TClient>(this IServiceCollection services) where TClient : class
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var name = TypeNameHelper.TypeNameHelper.GetTypeDisplayName(typeof(TClient), fullName: false);
        
        return AddHttpClientMockCore(services, name);
    }
    
    public static IHttpClientMockBuilder AddHttpClientMock(this IServiceCollection services, string clientName)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        return AddHttpClientMockCore(services, clientName);
    }

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