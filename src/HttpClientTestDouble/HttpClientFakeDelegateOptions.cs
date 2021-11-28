using HttpClientTestDouble.Actions;

namespace HttpClientTestDouble;

public class HttpClientFakeDelegateOptions
{
    public IList<Func<IServiceProvider, IHttpClientAction>> HttpClientActionFactories { get; } = new List<Func<IServiceProvider, IHttpClientAction>>();
}