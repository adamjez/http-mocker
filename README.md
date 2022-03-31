# Test double for HttpClient

HttpMocker is a testing layer for HttpClient library. The library is best suited when used with [ASP.NET Core integration tests framework](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Testing)

## Getting started

Install nuget package
```
dotnet add package HttpMocker
```

### Typed client
Mock response for http request type class as client of `HttpClient`.

The `HttpClient` is often configured with the extension method `AddHttpClient`. 

```csharp
services.AddHttpClient<YourExternalClientClass>();
```

In your tests override `HttpClient` behavior inside `YourExternalClientClass`.
```csharp
services.AddHttpClientMock<YourExternalClientClass>()
    .WithFallback(
        () => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Content") });
```

### Named client
Mock response for http request using named client.

The `HttpClient` is often configured with the extension method `AddHttpClient`.

```csharp
services.AddHttpClient<YourExternalClientClass>("client-name");
```

In your tests override `HttpClient` behavior inside `YourExternalClientClass` using overload which accepts client name.
```csharp
services.AddHttpClientMock("client-name")
    .WithFallback(
        () => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Content") });
```


### Global handler
Mock all registered http clients in `IServiceCollection`.

Multiple `HttpClient` services are registered in `IServiceCollection` using typed and/or named methods.

```csharp
services.AddHttpClient<YourExternalClientClass>();
services.AddHttpClient<YourExternalClientClass>("client-name");
```

In your tests override all `HttpClient` registered inside `IServiceCollection` using `AddGlobalHttpClientMock` extension method.
```csharp
services.AddGlobalHttpClientMock()
    .WithFallback(() => new HttpResponseMessage(HttpStatusCode.NotFound));
```

## HttpClient Middleware

The middleware is mechanisms to control respond to incoming `HttpRequestMessage`. 

The Http Mocker library contains multiple built-in middlewares that can be registered using extension methods:
* `WithFallback`
* `WithCapturing`

Custom middleware can be registered with extension method `WithMiddleware<TMiddleware>`. The `TMiddleware` type has to be registered in `IServiceCollection`.