using HttpMocker.Middlewares;

namespace HttpMocker;

internal class HttpClientFakeDelegateOptions
{
    public IList<Func<IServiceProvider, IHttpClientMiddleware>> HttpClientActionFactories { get; } = new List<Func<IServiceProvider, IHttpClientMiddleware>>();
}