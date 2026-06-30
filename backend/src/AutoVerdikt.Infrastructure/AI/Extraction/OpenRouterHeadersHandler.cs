using Microsoft.Extensions.Options;

namespace AutoVerdikt.Infrastructure.AI.Extraction;

internal sealed class OpenRouterHeadersHandler(IOptions<OpenRouterOptions> options) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var opts = options.Value;
        request.Headers.TryAddWithoutValidation("HTTP-Referer", opts.SiteUrl);
        request.Headers.TryAddWithoutValidation("X-Title", opts.SiteName);
        return base.SendAsync(request, cancellationToken);
    }
}
