using Keeply.Application.Interfaces;
using Keeply.Domain.Entities;

namespace Keeply.UnitTests.Fakes;

public class FakeDocumentRepository : IDocumentRepository
{
    private readonly List<Document> _documents = [];

    public FakeDocumentRepository(IEnumerable<Document>? documents = null)
    {
        if (documents is not null)
            _documents.AddRange(documents);
    }

    public Task<Document?> GetByIdAsync(Guid id, Guid userId) =>
        Task.FromResult(_documents.FirstOrDefault(d => d.Id == id && d.UserId == userId));

    public Task AddAsync(Document document)
    {
        _documents.Add(document);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Document document)
    {
        _documents.Remove(document);
        return Task.CompletedTask;
    }

    public Task<int> CountByUserIdAsync(Guid userId) =>
        Task.FromResult(_documents.Count(d => d.UserId == userId));
}
