using Keeply.Application.DTOs.Documents;
using Keeply.Application.Interfaces;
using Keeply.Domain.Entities;

namespace Keeply.Application.Services;

public class DocumentService
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IProductRepository _productRepository;

    public DocumentService(IDocumentRepository documentRepository, IProductRepository productRepository)
    {
        _documentRepository = documentRepository;
        _productRepository = productRepository;
    }

    public async Task<DocumentResponse?> UploadAsync(Guid userId, Guid productId, string fileName, Stream content)
    {
        var product = await _productRepository.GetByIdAsync(productId, userId);
        if (product is null)
            return null;

        var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        Directory.CreateDirectory(uploadsRoot);

        var storedName = $"{Guid.NewGuid()}_{fileName}";
        var filePath = Path.Combine(uploadsRoot, storedName);

        await using (var fileStream = File.Create(filePath))
        {
            await content.CopyToAsync(fileStream);
        }

        var document = new Document
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ProductId = productId,
            FileName = fileName,
            FilePath = filePath,
            UploadedAt = DateTime.UtcNow
        };

        await _documentRepository.AddAsync(document);
        return MapToResponse(document);
    }

    public async Task<(DocumentResponse? Response, string? FilePath)> GetByIdAsync(Guid id, Guid userId)
    {
        var document = await _documentRepository.GetByIdAsync(id, userId);
        if (document is null)
            return (null, null);

        return (MapToResponse(document), document.FilePath);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var document = await _documentRepository.GetByIdAsync(id, userId);
        if (document is null)
            return false;

        if (File.Exists(document.FilePath))
            File.Delete(document.FilePath);

        await _documentRepository.DeleteAsync(document);
        return true;
    }

    private static DocumentResponse MapToResponse(Document document)
    {
        return new DocumentResponse(document.Id, document.ProductId, document.FileName, document.UploadedAt);
    }
}
