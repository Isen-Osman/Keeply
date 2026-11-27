namespace Keeply.Application.DTOs.Dashboard;

public record DashboardSummaryResponse(
    int TotalProducts,
    int ActiveWarranties,
    int ExpiringWarranties,
    int ExpiredWarranties,
    int OpenClaims,
    int TotalDocuments);
