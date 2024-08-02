namespace Microservice.Experimental.Data.Models;

public sealed class SecureMessageAttachment
{
    public int Id { get; set; }
    public Guid DocumentId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string? ContentType { get; set; }
    public int ContentLength { get; set; }
    public int? SecureMessageId { get; set; }
    public int? ClientPartyId { get; set; }
    public int? TenantId { get; set; }
}