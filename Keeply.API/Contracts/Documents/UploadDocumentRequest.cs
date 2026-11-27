using System.ComponentModel.DataAnnotations;

namespace Keeply.API.Contracts.Documents;

public class UploadDocumentRequest
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public IFormFile File { get; set; } = default!;
}
