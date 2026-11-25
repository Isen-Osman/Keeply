using Keeply.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Keeply.Infrastructure.Persistence;

public class KeeplyDbContext : DbContext
{
    public KeeplyDbContext(DbContextOptions<KeeplyDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<WarrantyClaim> WarrantyClaims => Set<WarrantyClaim>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.UserId).IsRequired();
            entity.Property(p => p.Name).HasMaxLength(200).IsRequired();
            entity.Property(p => p.Store).HasMaxLength(200).IsRequired();
            entity.Property(p => p.Price).HasColumnType("numeric(18,2)");
            entity.Property(p => p.PurchaseDate).IsRequired();
            entity.Property(p => p.WarrantyMonths).IsRequired();
            entity.Ignore(p => p.WarrantyExpirationDate);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).HasMaxLength(200).IsRequired();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired();
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.UserId).IsRequired();
            entity.Property(d => d.ProductId).IsRequired();
            entity.Property(d => d.FileName).HasMaxLength(255).IsRequired();
            entity.Property(d => d.FilePath).HasMaxLength(500).IsRequired();
            entity.Property(d => d.UploadedAt).IsRequired();
        });

        modelBuilder.Entity<WarrantyClaim>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.UserId).IsRequired();
            entity.Property(c => c.ProductId).IsRequired();
            entity.Property(c => c.Description).HasMaxLength(1000).IsRequired();
            entity.Property(c => c.Status).HasMaxLength(50).IsRequired();
            entity.Property(c => c.CreatedAt).IsRequired();
        });
    }
}
