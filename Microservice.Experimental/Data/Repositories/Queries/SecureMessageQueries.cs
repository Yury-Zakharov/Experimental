namespace Microservice.Experimental.Data.Repositories.Queries;

public class SecureMessageQueries
{
    public const string GetById = @"
    select 
        [Id],
        [TenantId],
        [Subject],
        [Body],
        [BodyPreview],
        [Status],
        [SentTimeStamp],
        [ReceivedTimeStamp],
        [ReplySecureMessageId],
        [IsHtml],
        [IsRead],
        [IsReplied],
        [IsReadReceiptRequested],
        [ReadOnTimeStamp],
        [SenderName],
        [SenderPartyId],
        [SentMessageId],
        [ReceivedMessageId],
        [EmailNotificationTemplate],
        [UpdatedDraftTimeStamp],
        [ConversationId],
        [IsRevoked],
        [BulkMessageId]
    from [dbo].[SecureMessage]
    where
        [Id] = @id;";

    public const string GetAll = @"
    select 
        [Id],
        [TenantId],
        [Subject],
        [Body],
        [BodyPreview],
        [Status],
        [SentTimeStamp],
        [ReceivedTimeStamp],
        [ReplySecureMessageId],
        [IsHtml],
        [IsRead],
        [IsReplied],
        [IsReadReceiptRequested],
        [ReadOnTimeStamp],
        [SenderName],
        [SenderPartyId],
        [SentMessageId],
        [ReceivedMessageId],
        [EmailNotificationTemplate],
        [UpdatedDraftTimeStamp],
        [ConversationId],
        [IsRevoked],
        [BulkMessageId]
    from [dbo].[SecureMessage];";

    public const string Insert = @"
    insert into [dbo].[SecureMessage]
        (
        [Subject],
        [Body],
        [BodyPreview],
        [Status]
        )
    output
        inserted.[Id],
        inserted.[TenantId],
        inserted.[Subject],
        inserted.[Body],
        inserted.[BodyPreview],
        inserted.[Status],
        inserted.[SentTimeStamp],
        inserted.[ReceivedTimeStamp],
        inserted.[ReplySecureMessageId],
        inserted.[IsHtml],
        inserted.[IsRead],
        inserted.[IsReplied],
        inserted.[IsReadReceiptRequested],
        inserted.[ReadOnTimeStamp],
        inserted.[SenderName],
        inserted.[SenderPartyId],
        inserted.[SentMessageId],
        inserted.[ReceivedMessageId],
        inserted.[EmailNotificationTemplate],
        inserted.[UpdatedDraftTimeStamp],
        inserted.[ConversationId],
        inserted.[IsRevoked],
        inserted.[BulkMessageId]
    values
    (@subject,
    @body,
    @bodyPreview,
    @status
    );";
}