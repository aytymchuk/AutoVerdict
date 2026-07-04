using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Store.Research;

internal static class AttachedFileMapper
{
    public static AttachedFileDocument ToDocument(AttachedFile file, Guid researchId, string authId) =>
        new()
        {
            Id = file.Id,
            ResearchId = researchId,
            AuthId = authId,
            Kind = ToFileKindString(file.Kind),
            FileName = file.FileName,
            Url = file.Url,
            ThumbnailUrl = file.ThumbnailUrl,
            AgentDescription = file.AgentDescription,
            ProcessingStatus = ToProcessingStatusString(file.ProcessingStatus),
            RejectionReason = file.RejectionReason
        };

    public static AttachedFile ToDomain(AttachedFileDocument document) =>
        new()
        {
            Id = document.Id,
            Kind = ParseFileKind(document.Kind),
            FileName = document.FileName,
            Url = document.Url,
            ThumbnailUrl = document.ThumbnailUrl,
            AgentDescription = document.AgentDescription,
            ProcessingStatus = ParseProcessingStatus(document.ProcessingStatus),
            RejectionReason = document.RejectionReason
        };

    private static string ToFileKindString(FileKind kind) => kind switch
    {
        FileKind.Photo => "photo",
        FileKind.Document => "document",
        _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
    };

    private static FileKind ParseFileKind(string value) => value switch
    {
        "photo" => FileKind.Photo,
        "document" => FileKind.Document,
        _ => FileKind.Document
    };

    private static string ToProcessingStatusString(ProcessingStatus status) => status switch
    {
        ProcessingStatus.Pending => "pending",
        ProcessingStatus.Processing => "processing",
        ProcessingStatus.Completed => "completed",
        ProcessingStatus.Rejected => "rejected",
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };

    private static ProcessingStatus ParseProcessingStatus(string value) => value switch
    {
        "pending" => ProcessingStatus.Pending,
        "processing" => ProcessingStatus.Processing,
        "completed" => ProcessingStatus.Completed,
        "rejected" => ProcessingStatus.Rejected,
        _ => ProcessingStatus.Pending
    };
}
