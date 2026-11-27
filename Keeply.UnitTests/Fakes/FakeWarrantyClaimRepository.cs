using Keeply.Application.Interfaces;
using Keeply.Domain.Entities;

namespace Keeply.UnitTests.Fakes;

public class FakeWarrantyClaimRepository : IWarrantyClaimRepository
{
    private readonly List<WarrantyClaim> _claims = [];

    public FakeWarrantyClaimRepository(IEnumerable<WarrantyClaim>? claims = null)
    {
        if (claims is not null)
            _claims.AddRange(claims);
    }

    public Task<List<WarrantyClaim>> GetAllAsync(Guid userId)
    {
        return Task.FromResult(_claims.Where(c => c.UserId == userId).ToList());
    }

    public Task<WarrantyClaim?> GetByIdAsync(Guid id, Guid userId)
    {
        return Task.FromResult(_claims.FirstOrDefault(c => c.Id == id && c.UserId == userId));
    }

    public Task AddAsync(WarrantyClaim claim)
    {
        _claims.Add(claim);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(WarrantyClaim claim)
    {
        return Task.CompletedTask;
    }
}
