namespace Microservice.Experimental.Data.Models;

public sealed class SecureMessageRecipient
{
    public int Id { get; set; }
    public int? SecureMessageId { get; set; }
    public int? PartyId { get; set; }
    public string? Name { get; set; }
}