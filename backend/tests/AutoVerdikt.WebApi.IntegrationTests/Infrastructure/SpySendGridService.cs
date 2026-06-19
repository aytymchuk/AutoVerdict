using System.Collections.Concurrent;
using AutoVerdikt.Application.Email;

namespace AutoVerdikt.WebApi.IntegrationTests.Infrastructure;

public sealed class SpySendGridService : ISendGridService
{
    private readonly ConcurrentBag<ApprovalEmailCall> _calls = new();

    public IReadOnlyCollection<ApprovalEmailCall> Calls => _calls.ToList();

    public Task SendApprovalEmailAsync(string to, string locale, CancellationToken cancellationToken = default)
    {
        _calls.Add(new ApprovalEmailCall(to, locale));
        return Task.CompletedTask;
    }

    public void Clear() => _calls.Clear();
}

public sealed record ApprovalEmailCall(string To, string Locale);
