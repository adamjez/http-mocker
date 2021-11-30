using Microsoft.Extensions.DependencyInjection;

namespace HttpMocker;

public interface IHttpClientMockBuilder
{
    string Name { get; }
    IServiceCollection Services { get; }
}