namespace HttpMocker.Middlewares;

internal class FallbackMiddleware : IHttpClientMiddleware
{
    private readonly Func<HttpResponseMessage> _responseMessageFactory;

    public FallbackMiddleware(Func<HttpResponseMessage> responseMessageFactory)
    {
        _responseMessageFactory = responseMessageFactory;
    }

    public Task<HttpResponseMessage> Handle(HttpRequestMessage request, Func<HttpRequestMessage, Task<HttpResponseMessage>> next)
    {
        var response = _responseMessageFactory();
        response.RequestMessage = request;

        return Task.FromResult(response);
    }
}
