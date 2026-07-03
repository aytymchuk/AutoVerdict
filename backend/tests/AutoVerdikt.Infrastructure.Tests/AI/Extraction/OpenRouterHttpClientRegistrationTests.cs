using AutoVerdikt.Infrastructure.AI.Extraction;
using Microsoft.Extensions.Options;
using Shouldly;

namespace AutoVerdikt.Infrastructure.Tests.AI.Extraction;

public sealed class OpenRouterHttpClientRegistrationTests
{
    [Fact]
    public async Task OpenRouterHeadersHandler_adds_referer_and_title_headers()
    {
        var stub = new StubHandler();
        var handler = new OpenRouterHeadersHandler(Options.Create(new OpenRouterOptions
        {
            SiteUrl = "https://autoverdikt.com",
            SiteName = "AutoVerdikt"
        }))
        {
            InnerHandler = stub
        };

        var client = new HttpClient(handler) { BaseAddress = new Uri("https://openrouter.ai/api/v1/") };
        await client.GetAsync("chat/completions");

        stub.LastRequest.ShouldNotBeNull();
        stub.LastRequest!.Headers.TryGetValues(OpenRouterHeaderNames.Referer, out var refererValues).ShouldBeTrue();
        refererValues!.Single().ShouldBe("https://autoverdikt.com");
        stub.LastRequest.Headers.TryGetValues(OpenRouterHeaderNames.Title, out var titleValues).ShouldBeTrue();
        titleValues!.Single().ShouldBe("AutoVerdikt");
    }

    private sealed class StubHandler : DelegatingHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
        }
    }
}
