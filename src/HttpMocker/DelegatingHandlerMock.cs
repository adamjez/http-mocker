using HttpMocker.Actions;

namespace HttpMocker;

internal class DelegatingHandlerMock : DelegatingHandler
{
    private readonly IEnumerable<IHttpClientAction> _actions;

    public DelegatingHandlerMock(IEnumerable<IHttpClientAction> actions)
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