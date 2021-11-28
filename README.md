# Test double for Http Client

HttpClientTestDouble is a testing layer for HttpClient library. The library is best suited when used with [ASP.NET Core integration tests framework](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Testing)

## Examples
Create fake response for all http requests using class as client of `HttpClient`

```csharp
services.AddHttpClient<YourExternalClientClass>();
```

Later in your integration tests override `HttpClient` behaviour inside `YourExternalClientClass`
```csharp
services.AddHttpClientTestDouble<YourExternalClientClass>()
    .WithFallback(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Content") });
```
