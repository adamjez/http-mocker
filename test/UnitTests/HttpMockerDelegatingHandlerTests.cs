using Xunit;
using HttpMocker;
using System.Linq;
using HttpMocker.Middlewares;
using System.Net.Http;
using FluentAssertions;
using System;
using System.Threading.Tasks;

namespace UnitTests
{
    public class HttpMockerDelegatingHandlerTests
    {
        [Fact]
        public async Task ZeroMiddlewaresMockShouldThrowInvalidatOperation()
        {
            var mockHandler = new HttpMockerDelegatingHandler(
                Enumerable.Empty<IHttpClientMiddleware>());

            var client = new HttpClient(mockHandler);
            var action = () => client.SendAsync(CreateBasicGetRequest());

            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task NotHandledRequestShouldThrowInvalidatOperation()
        {
            var mockHandler = new HttpMockerDelegatingHandler(
                new[] { new NoopHttpClientMiddleware() });

            var client = new HttpClient(mockHandler);
            var action = () => client.SendAsync(CreateBasicGetRequest());

            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task FallbackMiddlewareShouldReturnPreparedResponse()
        {
            var mockHandler = new HttpMockerDelegatingHandler(
                new[] { new FallbackMiddleware(() => new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)) });

            var client = new HttpClient(mockHandler);
            using var response = await client.SendAsync(CreateBasicGetRequest());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task RequestProcessingPipelineShouldContinueUntilFirstSuccessfullMiddleware()
        {
            var mockHandler = new HttpMockerDelegatingHandler(
                new IHttpClientMiddleware[]
                {
                    new NoopHttpClientMiddleware(),
                    new FallbackMiddleware(() => new HttpResponseMessage(System.Net.HttpStatusCode.Found))
                });

            var client = new HttpClient(mockHandler);
            using var response = await client.SendAsync(CreateBasicGetRequest());

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Found);
        }

        [Fact]
        public async Task RequestCapturingMiddlewareShouldCaptureProcessedRequests()
        {
            var capturingMiddleware = new RequestCapturingMiddleware();
            var mockHandler = new HttpMockerDelegatingHandler(
                new IHttpClientMiddleware[]
                {
                    capturingMiddleware,
                    new FallbackMiddleware(() => new HttpResponseMessage(System.Net.HttpStatusCode.Found))
                });

            var client = new HttpClient(mockHandler);
            using var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://github.com"));

            capturingMiddleware.Requests.Should().HaveCount(1)
                .And.SatisfyRespectively(request =>
                {
                    request.Method.Should().Be(HttpMethod.Get);
                    request.RequestUri!.OriginalString.Should().Be("https://github.com");
                });
        }

        private static HttpRequestMessage CreateBasicGetRequest()
        {
            return new HttpRequestMessage(HttpMethod.Get, "https://github.com");
        }

        private class NoopHttpClientMiddleware : IHttpClientMiddleware
        {
            public Task<HttpResponseMessage> Handle(HttpRequestMessage request, Func<HttpRequestMessage, Task<HttpResponseMessage>> next)
            {
                return next(request);
            }
        }

    }
}