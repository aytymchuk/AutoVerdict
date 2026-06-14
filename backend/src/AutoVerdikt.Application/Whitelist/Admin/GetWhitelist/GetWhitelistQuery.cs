using AutoVerdikt.Application.Common;
using AutoVerdikt.Application.Whitelist;
using Mediator;

namespace AutoVerdikt.Application.Whitelist.Admin.GetWhitelist;

public sealed record GetWhitelistQuery(int Page, int PageSize) : IRequest<PaginatedResult<WhitelistListItem>>;
