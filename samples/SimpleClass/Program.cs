using System.Net;
using HttpClientTestDouble;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddHttpClient<ExternalClient>();

services.AddHttpClientFake<ExternalClient>()
    .WithFallback(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Content") });

var serviceProvider = services.BuildServiceProvider();

var externalClient = serviceProvider.GetRequiredService<ExternalClient>();

var response = await externalClient.Get();

Console.WriteLine(response); // Output: Content

internal class ExternalClient
{
    private readonly HttpClient _httpClient;

    public ExternalClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> Get()
    {
        return await _httpClient.GetStringAsync("https://google.com");
    }
}