namespace Keeply.Application.DTOs.Documents;

public record DocumentResponse(
    Guid Id,
    Guid ProductId,
    string FileName,
    DateTime UploadedAt);
