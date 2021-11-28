using HttpClientTestDouble.Actions;
using Microsoft.Extensions.DependencyInjection;

namespace HttpClientTestDouble;

internal class HttpClientFakeBuilder : IHttpClientFakeBuilder
{
    public HttpClientFakeBuilder(IServiceCollection services, string name)
    {
        Services = services;
        Name = name;
    }

    public string Name { get; }
    public IServiceCollection Services { get; }
}