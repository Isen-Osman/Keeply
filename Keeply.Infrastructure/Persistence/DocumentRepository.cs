using Keeply.Application.Interfaces;
using Keeply.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Keeply.Infrastructure.Persistence;

public class DocumentRepository : IDocumentRepository
{
    private readonly KeeplyDbContext _dbContext;

    public DocumentRepository(KeeplyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Document?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _dbContext.Documents.FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);
    }

    public async Task AddAsync(Document document)
    {
        _dbContext.Documents.Add(document);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Document document)
    {
        _dbContext.Documents.Remove(document);
        await _dbContext.SaveChangesAsync();
    }
}
