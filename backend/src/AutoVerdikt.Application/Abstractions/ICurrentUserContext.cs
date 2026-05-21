namespace AutoVerdikt.Application.Abstractions;

public interface ICurrentUserContext
{
    string AuthId { get; }
}
