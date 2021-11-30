using System.Net;
using HttpMocker;
using Microsoft.Extensions.DependencyInjection;

await MockTypedClient();
await MockNamedClient();
await MockGlobalClients();

async Task MockTypedClient()
{
    var services = new ServiceCollection();

    services.AddHttpClient<ExternalClient>();
    services.AddHttpClientMock<ExternalClient>()
        .WithFallback(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Class client") });

    var serviceProvider = services.BuildServiceProvider();

    var externalClient = serviceProvider.GetRequiredService<ExternalClient>();

    var response = await externalClient.Get();

    Console.WriteLine(response); // Output: Class client
}

async Task MockNamedClient()
{
    var services = new ServiceCollection();

    services.AddTransient<NamedExternalClient>();
    services.AddHttpClient("client name");

    services.AddHttpClientMock("client name")
        .WithFallback(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Named Client") });

    var serviceProvider = services.BuildServiceProvider();

    var namedExternalClient = serviceProvider.GetRequiredService<NamedExternalClient>();

    var response = await namedExternalClient.Get();

    Console.WriteLine(response); // Output: Named Client
}

async Task MockGlobalClients()
{
    var services = new ServiceCollection();

    services.AddHttpClient("client name");

    services.AddHttpClient<ExternalClient>();

    services.AddTransient<NamedExternalClient>();
    services.AddHttpClient("client name");

    services.AddGlobalHttpClientMock()
        .WithFallback(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Global!!!") });

    var serviceProvider = services.BuildServiceProvider();

    var externalClient = serviceProvider.GetRequiredService<ExternalClient>();

    Console.WriteLine(await externalClient.Get()); // Output: Global!!!
    
    var namedExternalClient = serviceProvider.GetRequiredService<NamedExternalClient>();

    Console.WriteLine(await namedExternalClient.Get()); // Output: Global!!!
}



internal class ExternalClient
{
    private readonly HttpClient _httpClient;

    public ExternalClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<string> Get() => await _httpClient.GetStringAsync("https://google.com");
}

internal class NamedExternalClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public NamedExternalClient(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public async Task<string> Get() => await _httpClientFactory
        .CreateClient("client name")
        .GetStringAsync("https://bing.com");
}