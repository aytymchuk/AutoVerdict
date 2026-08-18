using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Store.Research;

internal static class BadgeMapper
{
    public static BadgeDocument ToDocument(Badge badge, Guid researchId, string authId) =>
        new()
        {
            Id = badge.Id,
            ResearchId = researchId,
            AuthId = authId,
            Name = badge.Name,
            Status = ToBadgeStatusString(badge.Status),
            Description = badge.Description,
            Source = badge.Source
        };

    public static Badge ToDomain(BadgeDocument document) =>
        new()
        {
            Id = document.Id,
            Name = document.Name,
            Status = ParseBadgeStatus(document.Status),
            Description = document.Description,
            Source = document.Source
        };

    private static string ToBadgeStatusString(BadgeStatus status) => status switch
    {
        BadgeStatus.Good => "good",
        BadgeStatus.Warning => "warning",
        BadgeStatus.Critical => "critical",
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };

    private static BadgeStatus ParseBadgeStatus(string value) => value switch
    {
        "good" => BadgeStatus.Good,
        "warning" => BadgeStatus.Warning,
        "critical" => BadgeStatus.Critical,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unrecognized badge status")
    };
}
