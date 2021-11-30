using HttpMocker.Actions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace HttpMocker;

public static class HttpClientMockBuilderExtensions
{
    public static IHttpClientMockBuilder WithAction<TAction>(this IHttpClientMockBuilder builder)
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
    
    public static IHttpClientMockBuilder WithFallback(this IHttpClientMockBuilder builder, Func<HttpResponseMessage> responseMessageFactory)
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
    
    internal static void AddDelegatingHandlerFake(this IHttpClientMockBuilder builder)
    {
        builder.Services.PostConfigure<HttpClientFactoryOptions>(builder.Name, options =>
        {
            AddDelegatingHandlerFake(builder, options);
        });
    }
    
    internal static void AddGlobalDelegatingHandlerFake(this IHttpClientMockBuilder builder)
    {
        builder.Services.PostConfigureAll<HttpClientFactoryOptions>(options =>
        {
            AddDelegatingHandlerFake(builder, options);
        });
    }

    private static void AddDelegatingHandlerFake(IHttpClientMockBuilder builder, HttpClientFactoryOptions factoryOptions)
    {
        factoryOptions.HttpMessageHandlerBuilderActions.Add(handlerBuilder =>
        {
            var optionsMonitor =
                handlerBuilder.Services.GetRequiredService<IOptionsMonitor<HttpClientFakeDelegateOptions>>();
            var delegateOptions = optionsMonitor.Get(builder.Name);
            
            var actions = delegateOptions.HttpClientActionFactories.Select(factory => factory(handlerBuilder.Services));

            var delegatingHandler = new DelegatingHandlerMock(actions);
            handlerBuilder.AdditionalHandlers.Add(delegatingHandler);
        });
    }
}