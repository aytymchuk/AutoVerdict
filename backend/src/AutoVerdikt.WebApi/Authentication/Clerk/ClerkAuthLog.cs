using Microsoft.Extensions.Logging;

namespace AutoVerdikt.WebApi.Authentication.Clerk;

internal static partial class ClerkAuthLog
{
    [LoggerMessage(EventId = 2001, Level = LogLevel.Debug,
        Message = "JWT received: Authorization header present = {HasHeader}")]
    internal static partial void JwtReceived(ILogger logger, bool hasHeader);

    [LoggerMessage(EventId = 2002, Level = LogLevel.Warning,
        Message = "JWT authentication failed: {Error}")]
    internal static partial void JwtAuthenticationFailed(ILogger logger, string error);

    [LoggerMessage(EventId = 2003, Level = LogLevel.Warning,
        Message = "JWT rejected: azp '{Azp}' does not match authorized parties")]
    internal static partial void JwtAzpRejected(ILogger logger, string? azp);

    [LoggerMessage(EventId = 2004, Level = LogLevel.Warning,
        Message = "JWT challenge issued for {Path}: AuthenticateFailure = {Failure}")]
    internal static partial void JwtChallenge(ILogger logger, string path, string failure);
}
