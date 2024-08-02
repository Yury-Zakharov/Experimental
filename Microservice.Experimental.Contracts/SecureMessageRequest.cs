using System.ComponentModel.DataAnnotations;

namespace Microservice.Experimental.Contracts;

public readonly struct SecureMessageRequest
{
    public string Subject { get; init; }
    public string Body { get; init; }
    public int? ReceivedMessageId { get; init; }

    [Required]
    public string ContentType { get; init; }

    public bool IsReadReceiptRequested { get; init; }

    public string EmailNotificationTemplate { get; init; }
}