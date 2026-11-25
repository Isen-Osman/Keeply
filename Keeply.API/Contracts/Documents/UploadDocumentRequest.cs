namespace Keeply.API.Contracts.Documents;

public class UploadDocumentRequest
{
    public Guid ProductId { get; set; }
    public IFormFile File { get; set; } = default!;
}
