using HttpClientTestDouble.Actions;

namespace HttpClientTestDouble;

internal class DelegatingHandlerFake : DelegatingHandler
{
    private readonly IEnumerable<IHttpClientAction> _actions;

    public DelegatingHandlerFake(IEnumerable<IHttpClientAction> actions)
    {
        _actions = actions;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        foreach (var action in _actions)
        {
            if (action.CanHandle(request))
            {
                return action.GenerateResponse(request);
            }
        }

        throw new InvalidOperationException($"No action handled request to {request.Method} {request.RequestUri}");
    }
}