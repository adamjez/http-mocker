namespace HttpMocker.Middlewares;

public interface IHttpClientMiddleware
{
    Task<HttpResponseMessage> Handle(HttpRequestMessage request, Func<HttpRequestMessage, Task<HttpResponseMessage>> next);
}