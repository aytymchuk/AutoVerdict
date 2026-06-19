using AutoVerdikt.Application.Common;

namespace AutoVerdikt.Application.Whitelist.Admin.GetWhitelist;

public sealed class GetWhitelistQueryHandler(IWhitelistService whitelistService)
    : IRequestHandler<GetWhitelistQuery, PaginatedResult<WhitelistListItem>>
{
    public async ValueTask<PaginatedResult<WhitelistListItem>> Handle(
        GetWhitelistQuery query,
        CancellationToken cancellationToken)
        => await whitelistService.ListAsync(query.Page, query.PageSize, cancellationToken);
}
