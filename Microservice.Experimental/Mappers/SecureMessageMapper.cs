using Microservice.Experimental.Contracts;
using Microservice.Experimental.Data.Models;

namespace Microservice.Experimental.Mappers;

public static class SecureMessageMapper
{
    public static SecureMessage ToDomain(SecureMessageRequest contract) => new()
    {
        Body = contract.Body,
        BodyPreview = contract.Body[..Math.Min(0, contract.Body.Length)],
        EmailNotificationTemplate = contract.EmailNotificationTemplate,
        IsReadReceiptRequested = contract.IsReadReceiptRequested,
        Subject = contract.Subject,
        ReceivedMessageId = contract.ReceivedMessageId,
    };

    public static SecureMessageDocument ToContract(SecureMessage domain) => new()
    {
        Id = domain.Id,
        Body = domain.Body,
        BodyPreview = domain.BodyPreview,
        BulkMessageId = domain.BulkMessageId,
        EmailNotificationTemplate = domain.EmailNotificationTemplate,
        ConversationId = domain.ConversationId,
        IsHtml = domain.IsHtml,
        IsRead = domain.IsRead,
        IsReadReceiptRequested = domain.IsReadReceiptRequested,
        Subject = domain.Subject,
        ReceivedMessageId = domain.ReceivedMessageId,
        IsReplied = domain.IsReplied,
        IsRevoked = domain.IsRevoked,
        ReadOnTimeStamp = domain.ReadOnTimeStamp

    };

    public static SecureMessage MapToDomain(this SecureMessageRequest contract) => ToDomain(contract);

    public static SecureMessageDocument MapToContract(this SecureMessage domain) => ToContract(domain);
}