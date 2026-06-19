namespace AutoVerdikt.Application.Abstractions;

public interface ICurrentUserContext
{
    string AuthId { get; }
    string Email { get; }
    string Locale { get; }
    bool IsAdmin { get; }
}
