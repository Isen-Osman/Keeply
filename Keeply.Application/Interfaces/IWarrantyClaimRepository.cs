using Keeply.Domain.Entities;

namespace Keeply.Application.Interfaces;

public interface IWarrantyClaimRepository
{
    Task<List<WarrantyClaim>> GetAllAsync(Guid userId);
    Task<WarrantyClaim?> GetByIdAsync(Guid id, Guid userId);
    Task AddAsync(WarrantyClaim claim);
    Task UpdateAsync(WarrantyClaim claim);
}
