create table  [dbo].[SecureMessageRecipient]
(
    [Id] [int] IDENTITY(1,1) NOT NULL primary key clustered,
    [SecureMessageId] [int] NULL foreign key references [dbo].[SecureMessage]([Id]),
    [PartyId] [int] NULL,
    [Name] [nvarchar](500) NULL
);