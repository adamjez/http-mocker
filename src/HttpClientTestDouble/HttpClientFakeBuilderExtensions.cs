using HttpClientTestDouble.Actions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace HttpClientTestDouble;

public static class HttpClientFakeBuilderExtensions
{
    public static IHttpClientFakeBuilder WithAction<TAction>(this IHttpClientFakeBuilder builder)
        where TAction : class, IHttpClientAction
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.Configure<HttpClientFakeDelegateOptions>(builder.Name, options =>
        {
            options.HttpClientActionFactories.Add(serviceProvider => serviceProvider.GetRequiredService<TAction>());
        });

        return builder;
    }
    
    public static IHttpClientFakeBuilder WithFallback(this IHttpClientFakeBuilder builder, Func<HttpResponseMessage> responseMessageFactory)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.Configure<HttpClientFakeDelegateOptions>(builder.Name, options =>
        {
            options.HttpClientActionFactories.Add(_ => new FallbackAction(responseMessageFactory));
        });

        return builder;
    }
    
    internal static void AddDelegatingHandlerFake(this IHttpClientFakeBuilder builder)
    {
        builder.Services.PostConfigure<HttpClientFactoryOptions>(builder.Name, options =>
        {
            AddDelegatingHandlerFake(builder, options);
        });
    }
    
    internal static void AddGlobalDelegatingHandlerFake(this IHttpClientFakeBuilder builder)
    {
        builder.Services.PostConfigureAll<HttpClientFactoryOptions>(options =>
        {
            AddDelegatingHandlerFake(builder, options);
        });
    }

    private static void AddDelegatingHandlerFake(IHttpClientFakeBuilder builder, HttpClientFactoryOptions factoryOptions)
    {
        factoryOptions.HttpMessageHandlerBuilderActions.Add(handlerBuilder =>
        {
            var optionsMonitor =
                handlerBuilder.Services.GetRequiredService<IOptionsMonitor<HttpClientFakeDelegateOptions>>();
            var delegateOptions = optionsMonitor.Get(builder.Name);
            
            var actions = delegateOptions.HttpClientActionFactories.Select(factory => factory(handlerBuilder.Services));

            var delegatingHandler = new DelegatingHandlerFake(actions);
            handlerBuilder.AdditionalHandlers.Add(delegatingHandler);
        });
    }
}