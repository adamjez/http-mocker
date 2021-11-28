using HttpClientTestDouble.Actions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace HttpClientTestDouble;

public static class HttpClientTestDoubleBuilderExtensions
{
    public static IHttpClientTestDoubleBuilder WithFallback(this IHttpClientTestDoubleBuilder builder, Func<HttpResponseMessage> responseMessageFactory)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Actions.Add(new FallbackAction(responseMessageFactory));

        return builder;
    }
    
    internal static void AddDelegatingHandlerTestDouble(this IHttpClientTestDoubleBuilder builder, string name)
    {
        builder.Services.PostConfigure<HttpClientFactoryOptions>(name, options =>
        {
            options.HttpMessageHandlerBuilderActions.Add(handlerBuilder =>
            {
                var delegatingHandler = new DelegatingHandlerTestDouble(builder.Actions);
                handlerBuilder.AdditionalHandlers.Add(delegatingHandler);
            });
        });
    }
}