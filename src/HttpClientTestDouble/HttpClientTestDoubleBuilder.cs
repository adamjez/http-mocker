using HttpClientTestDouble.Actions;
using Microsoft.Extensions.DependencyInjection;

namespace HttpClientTestDouble;

internal class HttpClientTestDoubleBuilder : IHttpClientTestDoubleBuilder
{
    public HttpClientTestDoubleBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public List<IHttpClientAction> Actions { get; } = new();
    public IServiceCollection Services { get; }
}