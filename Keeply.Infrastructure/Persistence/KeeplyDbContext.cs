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
    }
}
