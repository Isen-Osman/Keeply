using Keeply.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Keeply.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ProductService>();
        services.AddScoped<AuthService>();
        services.AddScoped<WarrantyService>();
        services.AddScoped<DocumentService>();
        services.AddScoped<WarrantyClaimService>();
        return services;
    }
}
