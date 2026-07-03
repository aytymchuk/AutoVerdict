using Microsoft.Extensions.Options;

namespace AutoVerdikt.Infrastructure.AI.Extraction;

internal static class OpenRouterHeaderNames
{
    public const string Referer = "HTTP-Referer";
    public const string Title = "X-Title";
}

internal sealed class OpenRouterHeadersHandler(IOptions<OpenRouterOptions> options) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var opts = options.Value;
        request.Headers.TryAddWithoutValidation(OpenRouterHeaderNames.Referer, opts.SiteUrl);
        request.Headers.TryAddWithoutValidation(OpenRouterHeaderNames.Title, opts.SiteName);
        return base.SendAsync(request, cancellationToken);
    }
}
