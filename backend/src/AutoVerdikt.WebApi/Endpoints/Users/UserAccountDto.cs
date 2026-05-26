namespace AutoVerdikt.WebApi.Endpoints.Users;

// AuthId intentionally omitted — not exposed to API consumers
public sealed record UserAccountDto(Guid Id, string Name, string Email, DateTimeOffset RegisteredAt);
