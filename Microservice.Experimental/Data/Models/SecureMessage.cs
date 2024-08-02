namespace Microservice.Experimental.Data.Models;

public sealed class SecureMessage
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public string? BodyPreview { get; set; }
    public string Status { get; set; } = "Draft";
    public DateTime? SentTimeStamp { get; set; }
    public DateTime? ReceivedTimeStamp { get; set; }
    public int? ReplySecureMessageId { get; set; }
    public bool IsHtml { get; set; }
    public bool IsRead { get; set; }
    public bool IsReplied { get; set; }
    public bool IsReadReceiptRequested { get; set; }
    public DateTime? ReadOnTimeStamp { get; set; }
    public string? SenderName { get; set; }
    public int? SenderPartyId { get; set; }
    public int? SentMessageId { get; set; }
    public int? ReceivedMessageId { get; set; }
    public string? EmailNotificationTemplate { get; set; }
    public DateTime? UpdatedDraftTimeStamp { get; set; }
    public Guid? ConversationId { get; set; }
    public bool IsRevoked { get; set; }
    public int? BulkMessageId { get; set; }
}