using AutoVerdikt.Application.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AutoVerdikt.Infrastructure.Email;

public sealed class SendGridService(
    IOptions<SendGridOptions> options,
    ILogger<SendGridService> logger) : ISendGridService
{
    public async Task SendApprovalEmailAsync(
        string to,
        string locale,
        CancellationToken cancellationToken = default)
    {
        var opts = options.Value;
        if (string.IsNullOrWhiteSpace(opts.ApiKey) || string.IsNullOrWhiteSpace(opts.FromEmail))
        {
            logger.LogWarning("SendGrid is not configured; skipping approval email to {Email}", to);
            return;
        }

        var (subject, html) = ApprovalEmailTemplates.Get(locale);
        var client = new SendGridClient(opts.ApiKey);
        var from = new EmailAddress(opts.FromEmail, opts.SenderName);
        var msg = MailHelper.CreateSingleEmail(from, new EmailAddress(to), subject, plainTextContent: null, htmlContent: html);
        var response = await client.SendEmailAsync(msg, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Body.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException($"SendGrid returned {(int)response.StatusCode}: {body}");
        }
    }
}

internal static class ApprovalEmailTemplates
{
    internal static (string Subject, string Html) Get(string locale) => locale switch
    {
        "pl" => (
            "Masz dostęp do AutoCheck AI ✓",
            """
            <p>Cześć,</p>
            <p>Twoje zgłoszenie zostało zatwierdzone.<br/>
            Możesz teraz zalogować się i zacząć korzystać z AutoCheck AI.</p>
            <p><a href="/auth">Zaloguj się →</a></p>
            <p>Do zobaczenia w aplikacji,<br/>Zespół AutoCheck AI</p>
            """),
        "uk" => (
            "Доступ до AutoCheck AI відкрито ✓",
            """
            <p>Привіт,</p>
            <p>Вашу заявку підтверджено.<br/>
            Тепер можете увійти і почати користуватись AutoCheck AI.</p>
            <p><a href="/auth">Увійти →</a></p>
            <p>До зустрічі в додатку,<br/>Команда AutoCheck AI</p>
            """),
        _ => (
            "You're in — AutoCheck AI ✓",
            """
            <p>Hi,</p>
            <p>Your request has been approved.<br/>
            You can now sign in and start using AutoCheck AI.</p>
            <p><a href="/auth">Sign in →</a></p>
            <p>See you inside,<br/>The AutoCheck AI team</p>
            """)
    };
}
