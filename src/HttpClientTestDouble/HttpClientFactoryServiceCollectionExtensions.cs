using Microsoft.Extensions.DependencyInjection;

namespace HttpClientTestDouble;

public static class HttpClientFactoryServiceCollectionExtensions
{
    public static IHttpClientTestDoubleBuilder AddHttpClientTestDouble<TClient>(this IServiceCollection services) where TClient : class
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var name = TypeNameHelper.TypeNameHelper.GetTypeDisplayName(typeof(TClient), fullName: false);
        
        var builder = new HttpClientTestDoubleBuilder(services);
        builder.AddDelegatingHandlerTestDouble(name);

        return builder;
    }
}