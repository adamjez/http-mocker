namespace HttpMocker.Actions;

public class FallbackAction : IHttpClientAction
{
    private readonly Func<HttpResponseMessage> _responseMessageFactory;

    public FallbackAction(Func<HttpResponseMessage> responseMessageFactory)
    {
        _responseMessageFactory = responseMessageFactory;
    }

    public bool CanHandle(HttpRequestMessage request) => true;

    public Task<HttpResponseMessage> GenerateResponse(HttpRequestMessage request)
    {
        var response = _responseMessageFactory();
        response.RequestMessage = request;

        return Task.FromResult(response);
    }
}