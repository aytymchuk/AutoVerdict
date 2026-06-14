namespace AutoVerdikt.Application.Email;

public interface ISendGridService
{
    Task SendApprovalEmailAsync(string to, string locale, CancellationToken cancellationToken = default);
}
