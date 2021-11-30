using System.Net;
using HttpMocker;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddHttpClient<ExternalClient>();
services.AddHttpClient<AnotherExternalClient>();

services.AddGlobalHttpClientMock()
    .WithFallback(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Content") });

var serviceProvider = services.BuildServiceProvider();

var externalClient = serviceProvider.GetRequiredService<ExternalClient>();

var response = await externalClient.Get();

Console.WriteLine(response); // Output: Content

internal class ExternalClient
{
    private readonly HttpClient _httpClient;

    public ExternalClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<string> Get() => await _httpClient.GetStringAsync("https://google.com");
}

internal class AnotherExternalClient
{
    private readonly HttpClient _httpClient;

    public AnotherExternalClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<string> Get() => await _httpClient.GetStringAsync("https://bing.com");
}