namespace Keeply.Domain.Entities;

public class Document
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}
