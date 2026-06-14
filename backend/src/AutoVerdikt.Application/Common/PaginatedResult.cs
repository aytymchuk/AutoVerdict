namespace AutoVerdikt.Application.Common;

public sealed record PaginatedResult<T>(IReadOnlyList<T> Items, long Total);
