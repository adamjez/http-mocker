using HttpMocker.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace HttpMocker;

/// <summary>
/// Extension methods for registering HTTP client mock behavior into an <see cref="IHttpClientMockBuilder" />.
/// </summary>
public static class HttpClientMockBuilderExtensions
{
    /// <summary>
    /// Add generic middleware to HTTP client mock processing pipeline.
    /// </summary>
    /// <typeparam name="TMiddleware">Middleware type resolved from <see cref="IServiceProvider" /></typeparam>
    /// <param name="builder">The <see cref="IHttpClientMockBuilder" /> to add behavior to.</param>
    /// <returns>An <see cref="IHttpClientMockBuilder"/> for configuration chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <c>null</c>.</exception>
    public static IHttpClientMockBuilder WithMiddleware<TMiddleware>(this IHttpClientMockBuilder builder)
        where TMiddleware : class, IHttpClientMiddleware
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.Configure<HttpClientFakeDelegateOptions>(builder.Name, options =>
        {
            options.HttpClientActionFactories.Add(serviceProvider => serviceProvider.GetRequiredService<TMiddleware>());
        });

        return builder;
    }

    /// <summary>
    /// Add delegate to HTTP client mock processing pipeline.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientMockBuilder" /> to add behavior to.</param>
    /// <param name="responseMessageFactory">Invoked as part of processing pipeline</param>
    /// <returns>An <see cref="IHttpClientMockBuilder"/> for configuration chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <c>null</c>.</exception>
    public static IHttpClientMockBuilder WithFallback(this IHttpClientMockBuilder builder, Func<HttpResponseMessage> responseMessageFactory)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.Configure<HttpClientFakeDelegateOptions>(builder.Name, options =>
        {
            options.HttpClientActionFactories.Add(_ => new FallbackMiddleware(responseMessageFactory));
        });

        return builder;
    }

    /// <summary>
    /// Add services to capture incomming <see cref="HttpRequestMessage" />. 
    /// Requests can be retrieved from <see cref="ServiceProviderExtensions.GetCapturedRequests" /> extension method.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientMockBuilder" /> to add behavior to.</param>
    /// <returns>An <see cref="IHttpClientMockBuilder"/> for configuration chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <c>null</c>.</exception>
    public static IHttpClientMockBuilder WithCapturing(this IHttpClientMockBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.TryAddSingleton<RequestCapturingMiddleware>();

        builder.Services.Configure<HttpClientFakeDelegateOptions>(builder.Name, options =>
        {
            options.HttpClientActionFactories.Add(sp => sp.GetRequiredService<RequestCapturingMiddleware>());
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
            
            var actions = delegateOptions.HttpClientActionFactories
                .Select(factory => factory(handlerBuilder.Services));

            var delegatingHandler = new DelegatingHandlerMock(actions);
            handlerBuilder.AdditionalHandlers.Add(delegatingHandler);
        });
    }
}