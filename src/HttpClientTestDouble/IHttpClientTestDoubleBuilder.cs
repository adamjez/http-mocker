using HttpClientTestDouble.Actions;
using Microsoft.Extensions.DependencyInjection;

namespace HttpClientTestDouble;

public interface IHttpClientTestDoubleBuilder
{
    List<IHttpClientAction> Actions { get; }
    IServiceCollection Services { get; }
}