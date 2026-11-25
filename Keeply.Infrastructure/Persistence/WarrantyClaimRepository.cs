using Keeply.Application.Interfaces;
using Keeply.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Keeply.Infrastructure.Persistence;

public class WarrantyClaimRepository : IWarrantyClaimRepository
{
    private readonly KeeplyDbContext _dbContext;

    public WarrantyClaimRepository(KeeplyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<WarrantyClaim>> GetAllAsync(Guid userId)
    {
        return await _dbContext.WarrantyClaims.AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<WarrantyClaim?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _dbContext.WarrantyClaims.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
    }

    public async Task AddAsync(WarrantyClaim claim)
    {
        _dbContext.WarrantyClaims.Add(claim);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(WarrantyClaim claim)
    {
        _dbContext.WarrantyClaims.Update(claim);
        await _dbContext.SaveChangesAsync();
    }
}
