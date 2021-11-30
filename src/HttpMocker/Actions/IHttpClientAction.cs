namespace HttpMocker.Actions;

public interface IHttpClientAction
{
    bool CanHandle(HttpRequestMessage request);
    Task<HttpResponseMessage> GenerateResponse(HttpRequestMessage request);
}