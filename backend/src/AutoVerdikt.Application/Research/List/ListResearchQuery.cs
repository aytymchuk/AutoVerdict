using AutoVerdikt.Application.Common;

namespace AutoVerdikt.Application.Research.List;

public sealed record ListResearchQuery(int Page = 1, int PageSize = 20)
    : IRequest<Result<PaginatedResult<Domain.Research.ResearchRecord>>>;
