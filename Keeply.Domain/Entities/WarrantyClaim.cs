namespace Keeply.Domain.Entities;

public class WarrantyClaim
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public DateTime CreatedAt { get; set; }
}
