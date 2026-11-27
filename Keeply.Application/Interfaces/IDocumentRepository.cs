using Keeply.Domain.Entities;

namespace Keeply.Application.Interfaces;

public interface IDocumentRepository
{
    Task<Document?> GetByIdAsync(Guid id, Guid userId);
    Task AddAsync(Document document);
    Task DeleteAsync(Document document);
    Task<int> CountByUserIdAsync(Guid userId);
}
