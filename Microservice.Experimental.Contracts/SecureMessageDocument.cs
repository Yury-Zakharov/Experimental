using IntelliFlo.Platform.Http.Documentation.Annotations;

namespace Microservice.Experimental.Contracts;

/// <summary>
/// Secure message alternative.
/// </summary>
[SwaggerDefinition("SecureMessage")]
public sealed class SecureMessageDocument
{
    /// <summary>
    /// Secure message unique identifier.
    /// </summary>
    public int Id { get; init; }
    public int TenantId { get; init; }
    public string? Subject { get; init; }
    public string? Body { get; init; }
    public string? BodyPreview { get; init; }
    public SecureMessageStatus Status { get; init; }
    public DateTime? SentTimeStamp { get; init; }
    public DateTime? ReceivedTimeStamp { get; init; }
    public int? ReplySecureMessageId { get; init; }
    public bool IsHtml { get; init; }
    public bool IsRead { get; init; }
    public bool IsReplied { get; init; }
    public bool IsReadReceiptRequested { get; init; }
    public DateTime? ReadOnTimeStamp { get; init; }
    public string? SenderName { get; init; }
    public int? SenderPartyId { get; init; }
    public int? SentMessageId { get; init; }
    public int? ReceivedMessageId { get; init; }
    public string? EmailNotificationTemplate { get; init; }
    public DateTime? UpdatedDraftTimeStamp { get; init; }
    public Guid? ConversationId { get; init; }
    public bool IsRevoked { get; init; }
    public int? BulkMessageId { get; init; }
}