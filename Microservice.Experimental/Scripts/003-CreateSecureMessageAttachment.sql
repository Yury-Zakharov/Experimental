create table [dbo].[SecureMessageAttachment]
(
    [Id] [int] IDENTITY(1,1) NOT NULL primary key clustered,
    [DocumentId] [uniqueidentifier] NOT NULL,
    [FileName] [nvarchar](255) NOT NULL,
    [ContentLength] [int] NOT NULL,
    [SecureMessageId] [int] NULL foreign key references [dbo].[SecureMessage]([Id]),
    [ClientPartyId] [int] NULL,
    [TenantId] [int] NULL,
    [ContentType] [nvarchar](255) NULL
);