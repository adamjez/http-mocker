using System.Collections.Concurrent;

namespace HttpMocker.Middlewares;

internal class RequestCapturingMiddleware : IHttpClientMiddleware
{
    private readonly ConcurrentQueue<(HttpRequestMessage Request, HttpResponseMessage Response)> _storage = new();

    public IEnumerable<HttpRequestMessage> Requests => _storage.Select(s => s.Request);

    public async Task<HttpResponseMessage> Handle(HttpRequestMessage request, Func<HttpRequestMessage, Task<HttpResponseMessage>> next)
    {
        var response = await next(request);

        _storage.Enqueue((request, response));

        return response;
    }
}
