using HttpMocker.Middlewares;
using System.Collections.Immutable;

namespace HttpMocker;

internal class HttpMockerDelegatingHandler : DelegatingHandler
{
    private readonly ImmutableArray<IHttpClientMiddleware> _middlewares;

    public HttpMockerDelegatingHandler(IEnumerable<IHttpClientMiddleware> middlewares)
    {
        _middlewares = middlewares.ToImmutableArray();
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Task<HttpResponseMessage> Process(HttpRequestMessage innerRequest, ImmutableArray<IHttpClientMiddleware> middlewares)
        {
            var (head, tail) = Deconstruct(middlewares);

            if (head is null)
            {
                throw new InvalidOperationException($"No action handled request to {request.Method} {request.RequestUri}");
            }
            else
            {
                return head.Handle(innerRequest, r => Process(r, tail));
            }
        }

        return Process(request, _middlewares);
    }

    private static (T? head, ImmutableArray<T> tail) Deconstruct<T>(ImmutableArray<T> array)
    {
        var head = array.FirstOrDefault();
        var tail = head is null ? array : array.Remove(head);
        return (head, tail);
    }
}