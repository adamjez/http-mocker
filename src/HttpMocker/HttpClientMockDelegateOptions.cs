using HttpMocker.Actions;

namespace HttpMocker;

public class HttpClientFakeDelegateOptions
{
    public IList<Func<IServiceProvider, IHttpClientAction>> HttpClientActionFactories { get; } = new List<Func<IServiceProvider, IHttpClientAction>>();
}