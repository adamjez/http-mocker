using HttpMocker.Actions;
using Microsoft.Extensions.DependencyInjection;

namespace HttpMocker;

internal class HttpClientMockBuilder : IHttpClientMockBuilder
{
    public HttpClientMockBuilder(IServiceCollection services, string name)
    {
        Services = services;
        Name = name;
    }

    public string Name { get; }
    public IServiceCollection Services { get; }
}