# AutoVerdict API Error Codes

All API errors are returned as [RFC 9457 Problem Details](https://www.rfc-editor.org/rfc/rfc9457).

## Response Format

```json
{
  "type": "https://autoverdikt.com/errors/usr-001",
  "title": "Human-readable summary",
  "status": 409,
  "errorCode": "USR-001",
  "traceId": "00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01"
}
```

The `errorCode` field is the stable identifier for programmatic error handling on the client.  
The `type` URI follows the pattern `https://autoverdikt.com/errors/{code-lowercase}`.  
The `traceId` links the response to the corresponding server-side log entry in Seq.

## Conventions

- Codes are **permanent** — never reuse a code once assigned, even if the error class is removed.
- Add a new row to this table in the **same PR** that introduces the error class.
- Code ranges are reserved per domain category to avoid conflicts.
- Each error class must extend `DomainError` in `AutoVerdikt.Application/Errors/DomainError.cs`.

## Implementation Reference

```csharp
// Error class (Application layer)
public sealed class MyNewError()
    : DomainError("XYZ-001", "Descriptive message.", HttpStatusCode.UnprocessableEntity);

// Endpoint (WebApi layer) — no per-error type inspection needed
if (result.IsFailed)
    return result.ToProblemResult();
```

---

## Error Categories

### User Errors (USR — 001–099)

| Code    | HTTP Status  | Error Class                  | Description                                        |
|---------|--------------|------------------------------|----------------------------------------------------|
| USR-001 | 409 Conflict | `UserAlreadyRegisteredError` | Auth identity or email is already registered       |
