using AutoVerdikt.Application.Common;
using Mediator;

namespace AutoVerdikt.Application.Whitelist.Admin.GetWhitelist;

public sealed record GetWhitelistQuery(int Page, int PageSize) : IRequest<PaginatedResult<WhitelistListItem>>;
