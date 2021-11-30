# Test double for HttpClient

HttpMocker is a testing layer for HttpClient library. The library is best suited when used with [ASP.NET Core integration tests framework](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Testing)

## Getting started

Install library nuget package
```
dotnet add package HttpMocker
```

### Typed client
Mock response for all http requests type class as client of `HttpClient`.

Somewhere in your `Startup` method `HttpClient` is registered

```csharp
services.AddHttpClient<YourExternalClientClass>();
```

Later in your integration tests override `HttpClient` behaviour inside `YourExternalClientClass`
```csharp
services.AddHttpClientMock<YourExternalClientClass>()
    .WithFallback(
        () => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Content") });
```

### Named client
Mock response for all http requests using named client.

Somewhere in your `Startup` method `HttpClient` is registered

```csharp
services.AddHttpClient<YourExternalClientClass>("client-name");
```

Later in your integration tests override `HttpClient` behaviour inside `YourExternalClientClass` using overload which accepts client name
```csharp
services.AddHttpClientMock("client-name")
    .WithFallback(
        () => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Content") });
```


### All clients
TODO