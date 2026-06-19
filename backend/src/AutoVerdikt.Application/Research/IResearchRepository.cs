using AutoVerdikt.Application.Common;
using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Application.Research;

public interface IResearchRepository
{
    Task<Result> CreateAsync(ResearchRecord record, CancellationToken cancellationToken = default);
    Task<Result<ResearchRecord?>> GetByIdAndAuthIdAsync(
        Guid id,
        string authId,
        CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(ResearchRecord record, string authId, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, string authId, CancellationToken cancellationToken = default);
    Task<Result<PaginatedResult<ResearchRecord>>> ListByAuthIdAsync(
        string authId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
