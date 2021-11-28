using Microsoft.Extensions.DependencyInjection;

namespace HttpClientTestDouble;

public interface IHttpClientFakeBuilder
{
    string Name { get; }
    IServiceCollection Services { get; }
}